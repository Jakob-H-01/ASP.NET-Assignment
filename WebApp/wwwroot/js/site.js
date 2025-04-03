const toggleButtons = document.querySelectorAll('[data-type="toggle"]')
const views = []
toggleButtons.forEach(button => {
    const view = document.querySelector(button.dataset.target)
    views.push(view)
})

function toggleView(targetView) {
    views.forEach(view => {
        if (view.id !== targetView.id) {
            view.classList.add('hidden')
        }
    })
    targetView.classList.toggle('hidden')
}

toggleButtons.forEach(button => {
    const targetView = document.querySelector(button.dataset.target)
    button.addEventListener('click', () => toggleView(targetView))
})