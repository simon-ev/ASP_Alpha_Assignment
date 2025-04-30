document.addEventListener('DOMContentLoaded', function () {
  
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




    document.addEventListener('click', function (event) {
        const dropdownButtons = document.querySelectorAll('[data-type="dropdown"]');
        let clickedDropdown = null;

        dropdownButtons.forEach(button => {
            const targetId = button.getAttribute('data-target');
            const targetElement = document.querySelector(targetId);

            if (button.contains(event.target)) {
                clickedDropdown = targetElement;

                document.querySelectorAll('.context-menu').forEach(menu => {
                    if (menu !== targetElement) menu.classList.add('hidden');
                });

                targetElement.classList.toggle('hidden');
            }
        });

        if (!clickedDropdown && !event.target.closest('.context-menu')) {
            document.querySelectorAll('.context-menu').forEach(menu => {
                menu.classList.add('hidden');
            });
        }
    });




    const showModal = (targetId) => {
        const targetElement = document.querySelector(targetId);
        targetElement.classList.add('modal-show');
        document.getElementById('modal-overlay').classList.remove('hidden');
    };

    const hideModal = (targetId) => {
        const targetElement = document.querySelector(targetId);
        targetElement.classList.remove('modal-show');

        if (document.querySelectorAll('.modal.modal-show').length === 0) {
            document.getElementById('modal-overlay').classList.add('hidden');
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

    document.getElementById('modal-overlay').addEventListener('click', function (e) {
        if (e.target.id === 'modal-overlay') {
            document.querySelectorAll('.modal.modal-show').forEach(modal => {
                modal.classList.remove('modal-show');
            });
            document.getElementById('modal-overlay').classList.add('hidden');
        }
    });



    document.addEventListener('click', function (e) {
        if (e.target.closest('.delete-btn')) {
            const button = e.target.closest('.delete-btn');
            const projectId = button.getAttribute('data-id');
            if (!projectId) return;

            if (confirm("Are you sure you want to delete this project?")) {
                fetch('/Projects/Delete', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(projectId)
                })
                    .then(res => res.json())
                    .then(data => {
                        if (data.success) {
                            const card = document.querySelector(`[data-project-id="${data.id}"]`);
                            if (card) card.remove();
                        } else {
                            alert(data.message || 'Failed to delete project.');
                        }
                    });
            }
        }
    });
});

