const modalityList = document.querySelectorAll('.modality-checkbox');
const campusDiv = document.getElementById('campusDiv');
const timeDiv = document.getElementById('timeDiv');
const daysDiv = document.getElementById('daysDiv');

let campusVisibility = 0;
let timeBlockVisibility = 0;

modalityList.forEach(modality => {
    if (modality.checked && modality.dataset.hasCampuses === "True") {
        campusVisibility++;
        campusDiv.classList.remove('invisible');
    }
    if (modality.checked && modality.dataset.hasTimeblocks === "True") {
        timeBlockVisibility++;
        timeDiv.classList.remove('invisible');
        daysDiv.classList.remove('invisible');
    }

    modality.addEventListener('change', e => {
        if (e.currentTarget.checked) {
            if (modality.dataset.hasCampuses === "True") {
                campusVisibility++;
            }
            if (modality.dataset.hasTimeblocks === "True") {
                timeBlockVisibility++;
            }
        }
        else {
            if (modality.dataset.hasCampuses === "True") {
                campusVisibility--;
            }
            if (modality.dataset.hasTimeblocks === "True") {
                timeBlockVisibility--;
            }
        }

        //show or hide the campuses
        if (campusVisibility > 0) {
            campusDiv.classList.remove('invisible');
        }
        else {
            campusDiv.classList.add('invisible');
            campusDiv.querySelectorAll('input[type="checkbox"]').forEach(checkbox => checkbox.checked = false);
        }

        //show or hide the time blocks
        if (timeBlockVisibility > 0) {
            timeDiv.classList.remove('invisible');
            daysDiv.classList.remove('invisible');
        }
        else {
            timeDiv.classList.add('invisible');
            timeDiv.querySelectorAll('input[type="checkbox"]').forEach(checkbox => checkbox.checked = false);
            daysDiv.classList.add('invisible');
            daysDiv.querySelectorAll('input[type="checkbox"]').forEach(checkbox => checkbox.checked = false);
        }
    });
});