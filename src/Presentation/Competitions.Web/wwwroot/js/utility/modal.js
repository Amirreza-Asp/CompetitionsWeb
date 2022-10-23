const hideBtn = document.querySelector(".modal-close");
const modal = document.querySelector(".my-modal");

hideBtn.addEventListener("click", () => {
    modal.classList.remove("show");
});
