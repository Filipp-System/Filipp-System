const designer = document.querySelector("#designerHost");

function addActivity() {
    designer.showActivityPicker();
}

function createNewWorkflow() {
    if (confirm('T? R U SRSLY WANNA DISCARD THUZZZ CHANGES? PLX SINK TWU TAIMS PLX!'))
        designer.newWorkflow();
}

function importWorkflow() {
    designer.import();
}
