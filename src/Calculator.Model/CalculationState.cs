using System;

namespace Calculator.Models
{
    [Flags]
    public enum CalculationState
    {
        None = 0x00,
        CalculateNeededMaterial = 0x01,
        EstimateEmployeesNeeded = 0x02,
        CalculateProductionSpeed = 0x04,
        SelectMachineToCalculateFor = 0x08,
        SelectMethodForProductionStep = 0x16,
        SelectTool = 0x32,
    }
}