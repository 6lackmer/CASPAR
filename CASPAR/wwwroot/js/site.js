// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const dropdownList = document.querySelectorAll('.dropdown');

dropdownList.forEach(dropdown => {
    const button = dropdown.querySelector('.dropdown-button');
    const menu = dropdown.querySelector('.dropdown-menu');

    var dropdownOpen = false;

    button.addEventListener('click', () => {
        menu.classList.toggle('hidden');
        dropdownOpen = !dropdownOpen;
    });

    document.body.addEventListener('click', (event) => {
        if (dropdownOpen && event.target.closest('.dropdown-button') !== button) {
            menu.classList.add('hidden');
            dropdownOpen = false;
        }
    });
});