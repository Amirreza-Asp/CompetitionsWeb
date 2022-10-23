const checkboxes = document.querySelectorAll(".check-icon");
const textInput = document.getElementById("multi-select-text-input");
const valueInput = document.getElementById("multi-select-value-input");
const spans = document.querySelectorAll(".multi-select-item>span");
const selectList = document.querySelector(".multi-select-list");
const container = document.querySelector('.multi-select');
const items = document.querySelectorAll(".multi-select-item");

window.addEventListener('click', function (e) {
    if (document.querySelector('.multi-select').contains(e.target)) {
        textInput.blur();
        if (!selectList.contains(e.target))
            selectList.classList.toggle("active");
    } else {
        selectList.classList.remove("active")
    }
});


items.forEach((item, index) => {
    item.addEventListener("click", (e) => {
        if (!checkboxes[index].contains(e.target))
            checkboxes[index].click();
    })
})

for (let i = 0; i < checkboxes.length; i++) {
    checkboxes[i].addEventListener("click", () => {
        const span = spans[i];
        if (valueInput.value.includes(span.getAttribute("value"))) {
            checkboxes[i].classList.remove("active")
            removeValue(span);
        }
        else {
            checkboxes[i].classList.add("active")
            addValue(span)
        }
    });
}

function removeValue(item) {
    const value = item.getAttribute("value");
    const text = item.innerText;

    let inputValue = valueInput.value;
    inputValue = inputValue.replace(value, "");
    inputValue = inputValue.replace(",,", ",");
    valueInput.value = inputValue;

    fixInput(valueInput)

    let inputText = textInput.value;
    inputText = inputText.replace(text, "");
    inputText = inputText.replace(",,", ",");
    textInput.value = inputText;

    fixInput(textInput)

}

function addValue(item) {
    const value = item.getAttribute("value");
    const text = item.innerText;

    if (valueInput.value)
        valueInput.value += "," + value;
    else
        valueInput.value = value;

    if (textInput.value)
        textInput.value += "," + text;
    else
        textInput.value = text;

}

function fixInput(input) {
    if (input.value.trim() === ",")
        input.value = "";
    if (input.value.startsWith(","))
        input.value = input.value.substring(1, input.value.length);
    if (input.value[input.value.length - 1] === ",")
        input.value = input.value.substring(0, input.value.length - 1);
}

function checkSelected() {
    const values = valueInput.value;
    const valList = values.split(',');
    valList.forEach(val => {
        spans.forEach((span, index) => {
            if (span.getAttribute("value") === val)
                checkboxes[index].classList.add("active")
        });
    });
}


window.addEventListener("DOMContentLoaded", checkSelected);


