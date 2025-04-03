const toggleButtons = document.querySelectorAll('[data-type="toggle"]')
const views = []
toggleButtons.forEach(button => {
    const view = document.querySelector(button.dataset.target)
    views.push(view)
})
const modalButtons = document.querySelectorAll('[data-type="modal"]')

function toggleDropdown(element, targetView) {
    views.forEach(view => {
        if (view.id !== targetView.id) {
            view.classList.add('hidden')
        }
    })

    toggleButtons.forEach(button => {
        if (button.dataset.target !== element.dataset.target) {
            button.classList.remove('active')
        }
    })

    targetView.classList.toggle('hidden')
    element.classList.toggle('active')
}

function toggleModal(modal) {
    modal.classList.toggle('hidden')
}

toggleButtons.forEach(button => {
    const targetView = document.querySelector(button.dataset.target)
    button.addEventListener('click', () => toggleDropdown(button, targetView))
})
modalButtons.forEach(button => {
    const targetModal = document.querySelector(button.dataset.target)
    button.addEventListener('click', () => toggleModal(targetModal))
})