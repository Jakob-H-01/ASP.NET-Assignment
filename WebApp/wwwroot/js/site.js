﻿const toggleButtons = document.querySelectorAll('[data-type="toggle"]')
const views = []
toggleButtons.forEach(button => {
    const view = document.querySelector(button.dataset.target)
    views.push(view)
})
const modalButtons = document.querySelectorAll('[data-type="modal"]')
const darkModeSwitch = document.querySelector('#dark-mode-switch')
const darkModeInput = document.querySelector('#dark-mode-input')

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

function toggleDarkMode() {
    darkModeInput.click()
    document.documentElement.classList.toggle('dark-mode')
    const isEnabled = localStorage.getItem('dark-mode')

    if (isEnabled === 'false' || isEnabled === null) {
        localStorage.setItem('dark-mode', 'true')
    } else if (isEnabled === 'true') {
        localStorage.setItem('dark-mode', 'false')
    }
}

function ifShouldBeDarkMode() {
    const isEnabled = localStorage.getItem('dark-mode')

    if (isEnabled === 'true') {
        darkModeInput?.click()
        document.documentElement.classList.add('dark-mode')
        localStorage.setItem('dark-mode', 'true')
    }
}

toggleButtons.forEach(button => {
    const targetView = document.querySelector(button.dataset.target)
    button.addEventListener('click', () => toggleDropdown(button, targetView))
})
modalButtons.forEach(button => {
    const targetModal = document.querySelector(button.dataset.target)
    button.addEventListener('click', () => toggleModal(targetModal))
})
darkModeSwitch?.addEventListener('click', toggleDarkMode)

ifShouldBeDarkMode()