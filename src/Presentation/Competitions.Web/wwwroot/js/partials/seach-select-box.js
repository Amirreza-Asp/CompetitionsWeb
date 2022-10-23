function ToggleOptions(item) {
    const options = item.parentElement.parentElement.nextElementSibling;
    options.classList.toggle("active");
}

function HideOptions(item) {
    alert("dfnsdl")
    const options = item.nextElementSibling;
    console.log(options)
    options.classList.remove("active");
}

var typingTimer;                //timer identifier
var doneTypingInterval = 1000;  //time in ms, 5 seconds for example
var $input = $('#myInput');

//on keyup, start the countdown

function KeyUp(input) {
    clearTimeout(typingTimer);
    typingTimer = setTimeout(() => SearchOptions(input), doneTypingInterval);
}

function KeyDown(input) {
    clearTimeout(typingTimer);
}

function SearchOptions(item) {
    const options = item.parentElement.parentElement.nextElementSibling;
    const lis = options.children;

    Array.from(lis).forEach(li => {
        const div = li.children[0];
        div.setAttribute("style", "display:initial");
        if (!div.innerText.includes(item.value))
            div.setAttribute("style", "display:none");

    })
}

function SetOption(item) {
    const value = item.children[0].getAttribute("value");
    const searchInput = item.parentElement.previousElementSibling.children[0].children[0];
    const targetInput = item.parentElement.previousElementSibling.previousElementSibling;

    searchInput.value = item.innerText;
    targetInput.setAttribute("value", value);
    targetInput.dispatchEvent(new Event("change"));

    item.parentElement.classList.remove("active");
}