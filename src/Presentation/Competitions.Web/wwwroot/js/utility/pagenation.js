function PreventDefault(event) {
	event.preventDefault();
}

function ChangePage(event, skip) {
	PreventDefault(event);
	const form = document.querySelector("form");

	const pageInput = document.createElement("input");
	pageInput.setAttribute("name", "skip");
	pageInput.setAttribute("value", `${skip}`);
	pageInput.setAttribute("hidden", `true`);
	form.appendChild(pageInput);

	const submit = document.createElement("input");
	submit.setAttribute("type", "submit");
	form.appendChild(submit);

	submit.click();
	submit.remove();
}

function ChangeTake(e) {
	Array.from(e.target.children).forEach(item => {
		if (item.getAttribute("selected") == "selected") {
			const form = document.querySelector("form");

			const pageInput = document.createElement("input");
			pageInput.setAttribute("name", "take");
			pageInput.setAttribute("value", `${item.getAttribute("value")}`);
			pageInput.setAttribute("hidden", `true`);

			const submit = document.createElement("input");
			submit.setAttribute("type", "submit");
			form.appendChild(submit);

			submit.click();
			submit.remove();
        }
    })
}