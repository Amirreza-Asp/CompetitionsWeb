const fileInput = document.querySelector("#file");
const showFileBtn = document.querySelector("#file-btn");
fileInput.addEventListener("change", (e) => {

	const files = fileInput.files.length;
	showFileBtn.innerHTML = `${files} فایل اضافه شد`;

})