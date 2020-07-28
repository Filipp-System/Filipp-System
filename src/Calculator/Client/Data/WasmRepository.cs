using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Calculator.BaseRepository;
using Calculator.Models;
using Calculator.Models.DatabaseModels;
using Task = System.Threading.Tasks.Task;

namespace Calculator.Client.Data
{
    /// <summary>
    /// Client implementation of the <see cref="IBasicRepository{Calculation}"/>.
    /// </summary>
    public class WasmRepository : IBasicRepository<Employee>
    {
        private readonly HttpClient _apiClient;
        private readonly IEmployeeFilters _controls;

        private const string ApiPrefix = "/api/";
        private string ApiEmployees => $"{ApiPrefix}employees/";
        private string ApiQuery => $"{ApiPrefix}query/";
        private string ForUpdate => "?forUpdate=true";

        /// <summary>
        /// Calculation as loaded then modified by the user.
        /// </summary>
        public Employee OriginalCalculation { get; set; }

        /// <summary>
        /// Calculation on the database
        /// </summary>
        public Employee DatabaseCalculation { get; set; }

        /// <summary>
        /// The row version of the last calculation loaded.
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// This will serialize a response from JSON and return null
        /// if the status code is 404 - not found.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<TEntity> SafeGetFromJsonAsync<TEntity>(string url)
        {
            var result = await _apiClient.GetAsync(url);
            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }

            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<TEntity>();
        }

        public WasmRepository(IHttpClientFactory clientFactory, IEmployeeFilters controls)
        {
            _apiClient = clientFactory.CreateClient(Program.BaseClient);
            _controls = controls;
        }

        public Task QueryAsync(Func<IQueryable<Employee>, Task> query)
        {
            return GetListAsync();
        }

        /// <summary>
        /// Gets a page of <see cref="Calculator.Models.DatabaseModels.Calculation"/> items.
        /// </summary>
        /// <returns>The result <see cref="ICollection{Calculation}"/>.</returns>
        public async Task<ICollection<Employee>> GetListAsync()
        {
            var result = await _apiClient.PostAsJsonAsync(ApiQuery, _controls);
            var queryInfo = await result.Content.ReadFromJsonAsync<QueryResult>();

            // transfer page information
            _controls.PageHelper.Refresh(queryInfo.PageInfo);
            return queryInfo.Employees;
        }

        /// <summary>
        /// Load an <see cref="Calculator.Models.DatabaseModels.Calculation"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="Calculator.Models.DatabaseModels.Calculation"/> to load.</param>
        /// <param name="_">Unused <see cref="ClaimsPrincipal"/>.</param>
        /// <param name="forUpdate"><c>True</c> when concurrency information should be loaded.</param>
        /// <returns></returns>
        public Task<Employee> LoadAsync(int id, ClaimsPrincipal _, bool forUpdate = false)
        {
            if (forUpdate)
            {
                return LoadAsync(id);
            }

            return SafeGetFromJsonAsync<Employee>($"{ApiEmployees}{id}");
        }

        /// <summary>
        /// Load an <see cref="Calculator.Models.DatabaseModels.Calculation"/> for updates.
        /// </summary>
        /// <param name="id">The id of the <see cref="Calculator.Models.DatabaseModels.Calculation"/> to load.</param>
        /// <returns></returns>
        public async Task<Employee> LoadAsync(int id)
        {
            OriginalCalculation = null;
            DatabaseCalculation = null;
            RowVersion = null;

            var result = await SafeGetFromJsonAsync<EmployeeConcurrencyResolver>($"{ApiEmployees}{id}{ForUpdate}");

            if (result == null)
            {
                return null;
            }

            // our instance
            OriginalCalculation = result.OriginalCalculation;

            // save the version
            RowVersion = result.RowVersion;

            return result.OriginalCalculation;
        }

        /// <summary>
        /// Delete an <see cref="Calculator.Models.DatabaseModels.Calculation"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="Calculator.Models.DatabaseModels.Calculation"/>.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param
        /// <returns><c>True</c> when successfully deleted.</returns>
        public async Task<bool> DeleteAsync(int id, ClaimsPrincipal user)
        {
            try
            {
                await _apiClient.DeleteAsync($"{ApiEmployees}{id}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="item"></param>
        public void Attach(Employee item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add an calculation
        /// </summary>
        /// <param name="item">The <see cref="Calculator.Models.DatabaseModels.Calculation"/> to add.</param>
        /// <param name="user">The logged in <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>The added <see cref="Calculator.Models.DatabaseModels.Calculation"/>.</returns>
        public async Task<Employee> AddAsync(Employee item, ClaimsPrincipal user)
        {
            var result = await _apiClient.PostAsJsonAsync(ApiEmployees, item);
            return await result.Content.ReadFromJsonAsync<Employee>();
        }

        /// <summary>
        /// Update an <see cref="Calculator.Models.DatabaseModels.Calculation"/> with concurrency checks.
        /// </summary>
        /// <param name="item">The <see cref="Calculator.Models.DatabaseModels.Calculation"/> to update.</param>
        /// <param name="user">The <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>The updated <see cref="Calculator.Models.DatabaseModels.Calculation"/>.</returns>
        public async Task<Employee> UpdateAsync(Employee item, ClaimsPrincipal user)
        {
            // send down the calculation with the version that have been tracked
            var result = await _apiClient.PutAsJsonAsync($"{ApiEmployees}{item.EmployeeID}", item.ToConcurrencyResolver(this));
            if (result.IsSuccessStatusCode)
            {
                return null;
            }

            if (result.StatusCode == HttpStatusCode.Conflict)
            {
                // concurrency issue, extract what the updated information is
                var resolver = await result.Content.ReadFromJsonAsync<EmployeeConcurrencyResolver>();
                DatabaseCalculation = resolver.DatabaseCalculation;
                var exception = new RepositoryConcurrencyException<Employee>(item, new Exception())
                {
                    DbEntity = resolver.DatabaseCalculation
                };
                RowVersion = resolver.RowVersion;
                throw exception;
            }

            throw new HttpRequestException($"Bad status code: {result.StatusCode}");
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public Task<TPropertyType> GetPropertyValueAsync<TPropertyType>(Employee item, string propertyName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task SetOriginalValueForConcurrencyAsync<TPropertyType>(Employee item, string propertyName, TPropertyType value)
        {
            throw new NotImplementedException();
        }
    }
}
