const addProjectDescriptionTextarea = document.getElementById('add-project-description');
const addProjectDescriptionQuill = new Quill('#add-project-description-wysiwyg-editor', {
    modules: {
        syntax: true,
        toolbar: false
    },
    theme: 'snow'
});

document.querySelectorAll('.toolbar-btn').forEach(button => {
    button.addEventListener('click', function () {
        const format = button.getAttribute('data-format');
        const value = button.getAttribute('data-value') || true;


        if (format === 'link') {
            const url = prompt('Enter the URL:');
            if (url) {
                addProjectDescriptionQuill.format('link', url);
            } else {
                addProjectDescriptionQuill.format('link', false);
            }
        } else {

            addProjectDescriptionQuill.format(format, value);
        }
    });
});

addProjectDescriptionQuill.on('text-change', function () {
    addProjectDescriptionTextarea.value = addProjectDescriptionQuill.root.innerHTML;
});



const dropdown = document.querySelectorAll('[data-type="dropdown"]')

document.addEventListener('click', function (event) {
    let clickedDropdown = null

    dropdown.forEach(dropdown => {
        const targetId = dropdown.getAttribute('data-target');
        const targetElement = document.querySelector(targetId);

        if (dropdown.contains(event.target)) {
            clickedDropdown = targetElement;

            document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                if (openDropdown !== targetElement) {
                    openDropdown.classList.remove('dropdown-show');
                }
            });

            targetElement.classList.toggle('dropdown-show');
        }
    });
    if (!clickedDropdown && !event.target.closest('.dropdown')) {
        document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
            openDropdown.classList.remove('dropdown-show')
        })
    }
})




const showModal = (targetId) => {
    const targetElement = document.querySelector(targetId);
    targetElement.classList.add('modal-show');
    document.getElementById('modal-overlay').style.display = 'block';
};

const hideModal = (targetId) => {
    const targetElement = document.querySelector(targetId);
    targetElement.classList.remove('modal-show');


    if (document.querySelectorAll('.modal.modal-show').length === 0) {
        document.getElementById('modal-overlay').style.display = 'none';
    }
};


document.querySelectorAll('[data-modal-open]').forEach(button => {
    button.addEventListener('click', function () {
        const targetId = button.getAttribute('data-modal-open');
        showModal(targetId);
    });
});


document.querySelectorAll('[data-modal-close]').forEach(button => {
    button.addEventListener('click', function () {
        const targetId = button.getAttribute('data-modal-close');
        hideModal(targetId);
    });
});

document.getElementById('modal-overlay').addEventListener('click', function () {
    document.querySelectorAll('.modal.modal-show').forEach(modal => {
        modal.classList.remove('modal-show');
    });
    document.getElementById('modal-overlay').style.display = 'none';
});


document.querySelectorAll('.form-select').forEach(select => {
    const trigger = select.querySelector('.form-select-trigger')
})



