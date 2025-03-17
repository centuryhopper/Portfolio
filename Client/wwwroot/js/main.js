// a nice trick to avoid clogging up the window object
CUSTOM = {
    testlog: (value) => {
        console.log(value);
    },
    copyToClipBoard: () => {
        const outputTextarea = document.getElementById('outputTextarea');
        // Copy the input value to the clipboard using navigator api
        navigator.clipboard.writeText(outputTextarea.value)
        .then(() => {
            console.log("Copied to clipboard: " + outputTextarea.value);
        })
        .catch(err => {
            console.error("Failed to copy: ", err);
        });
    },
    extractText: () => {
        const imageInput = document.getElementById('imageInput');
        const outputTextarea = document.getElementById('outputTextarea');

        //console.log(imageInput.files);

        if (imageInput.files.length === 0)
        {
            alert('please upload an image');
            return;
        }

        const file = imageInput.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = () => {
                const img = new Image();
                img.src = reader.result;
                img.onload = () => {
                    Tesseract.recognize(img)
                        .then(({ data: { text } }) => {
                            outputTextarea.value = text;
                        })
                        .catch((error) => {
                            console.error('Error:', error);
                        });
                };
            };
            reader.readAsDataURL(file);
        }
    },

}


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