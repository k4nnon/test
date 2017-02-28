
function Confirm() {
    var confirm_value = document.createElement("INPUT");
    confirm_value.type = "hidden";
    confirm_value.name = "confirm_value";
    if (confirm("¿Esta acción puede modificar el codigo del registro, desea continuar?")) {
        confirm_value.value = "Si";
    } else {
        confirm_value.value = "No";
    }
    if (confirm_value.value === "Si") {
        confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";
        if (confirm("Esta seguro de continuar?")) {
            confirm_value.value = "Si";
        } else {
            confirm_value.value = "No";
        }
    }
    document.forms[0].appendChild(confirm_value);
}

window.onload = function () {
    var scrollY = parseInt('<%=Request.Form["scrollY"] %>');
    if (!isNaN(scrollY)) {
        window.scrollTo(0, scrollY);
    }
};
window.onscroll = function () {
    var scrollY = document.body.scrollTop;
    if (scrollY === 0) {
        if (window.pageYOffset) {
            scrollY = window.pageYOffset;
        }
        else {
            scrollY = (document.body.parentElement) ? document.body.parentElement.scrollTop : 0;
        }
    }
    if (scrollY > 0) {
        var input = document.getElementById("scrollY");
        if (input === null) {
            input = document.createElement("input");
            input.setAttribute("type", "hidden");
            input.setAttribute("id", "scrollY");
            input.setAttribute("name", "scrollY");
            document.forms[0].appendChild(input);
        }
        input.value = scrollY;
    }
};

function GetSelectedDates() {
    var selectedDates = calMulti.selectedDates;
    var selectedDatesString = ""
    for (i = 0; i < selectedDates.length; i++) {
        selectedDatesString += "\n" + selectedDates[i];
    }
    alert("The selected dates are:\n" + selectedDatesString);
}