//import "./utility/collapse.js";
//import "./utility/dropdown.js";
//import "./utility/pagenation.js";
//import "./utility/pdf.js";

function ShowFilters(id) {
    const filterSection = document.getElementById(id);
    filterSection.classList.toggle('hide');
}

//collapse
document.querySelectorAll(".sidebar__collapse").forEach(item => {
    const sibling = item.nextElementSibling
    const hasActiveItem = sibling.querySelector(".active")
    if (hasActiveItem) sibling.style.maxHeight = `${sibling.scrollHeight}px`

    item.addEventListener("click", (e) => {
        e.currentTarget.classList.toggle("open")
        if (sibling.style.maxHeight === "0px")
            sibling.style.maxHeight = `${sibling.scrollHeight}px`
        else sibling.style.maxHeight = "0px"
    })
})

//dropdown
window.addEventListener('click', function (e) {
    if (document.getElementById('dropdown-auth') != null) {
        if (document.getElementById('dropdown-auth').contains(e.target)) {
            if (document.querySelector(".header__dropdown-info"))
                document.querySelector(".header__dropdown-info").classList.toggle("open")
        } else {
            if (document.querySelector(".header__dropdown-info"))
                document.querySelector(".header__dropdown-info").classList.remove("open")
        }
    }
});



// prevent up and down icons inside number input type
document.querySelectorAll("[type=number]").forEach(item => {
    item.addEventListener("keydown", (e) => {
        if (e.keyCode === 38 || e.keyCode === 40 || e.keyCode === 189) {
            e.preventDefault()
        }
    })
})

// check for persianAlphabet
const inputCheck = (item) => {
    if (item.getAttribute("id") === "login-username" || item.getAttribute("id") === "captcha")
        return;
    item.addEventListener("keydown", (e) => {
        const persianAlphabet = new RegExp("^[\u0600-\u06FF\\s]+$");
        const englishAlphabet = e.keyCode >= 65 && e.keyCode <= 90
        if (e.keyCode === 8 || !englishAlphabet || persianAlphabet.test(e.key)) return true;
        window.alert("لطفا حالت صفحه کليد خود را به فارسی تغيير دهيد.")
        e.preventDefault();
        return false;
    })
}

//apply inputCheck for input text type and textarea
document.querySelectorAll("input[type=text]").forEach(item => { inputCheck(item) })
document.querySelectorAll("textarea").forEach(item => { inputCheck(item) })


//sidebar toggle
const sidebar = document.querySelector(".sidebar")
const backdrop = document.querySelector(".backdrop")
const mobileMenu = document.querySelector(".mobile")

mobileMenu.addEventListener("click", () => {
    sidebar.classList.toggle("active")
    backdrop.classList.toggle("active")
})

if (backdrop !== null) {
    backdrop.addEventListener("click", (item) => {
        if (item.currentTarget.classList.contains("active")) {
            item.currentTarget.classList.remove("active")
            sidebar.classList.toggle("active")
        }
    })
}