
if (document.querySelector("#dropdown") != null) {
    document.querySelector("#dropdown").addEventListener("click", (e) => {
        document.querySelector(".header__dropdown-info").classList.toggle("open")
    })
}
