
function blazorFocusElement(element){
    if (element) {
        element.focus();
    }
};

function showHidePasswordField(fieldId)
{
    document.querySelector(`#${fieldId} a`).addEventListener('click', function(event) {
        event.preventDefault();
        //console.log(fieldId);
        const input = document.querySelector(`#${fieldId} input`);
        const icon = document.querySelector(`#${fieldId} i`);
    
        if (input.getAttribute("type") === "text")
        {
            input.setAttribute('type', 'password');
            icon.classList.add("fa-eye-slash");
            icon.classList.remove("fa-eye");
        }
        else if (input.getAttribute("type") === "password")
        {
            input.setAttribute('type', 'text');
            icon.classList.remove("fa-eye-slash");
            icon.classList.add("fa-eye");
        }
    });

}