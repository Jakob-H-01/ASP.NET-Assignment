const settingsDropdown = document.querySelector('.settings-dropdown')
const settingsButton = document.querySelector('#settings')

function openDropdown() {
    settingsDropdown.classList.toggle('hidden')
}

settingsButton.addEventListener('click', openDropdown)