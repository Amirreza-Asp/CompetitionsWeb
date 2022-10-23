function IncHours(inputId, hoursId) {
	const input = document.getElementById(inputId);
	let hours = document.getElementById(hoursId);

	let num = Number.parseInt(hours.innerHTML);
	if (num == 23) num = 0;
	else num++;

	if (num < 10) num = `0${num}`;

	const value = input.value;
	timesSpilt = value.split(":");

	hours.innerHTML = num;
	input.value = num + ":" + timesSpilt[1];
}

function DecHours(inputId, hoursId) {
	const input = document.getElementById(inputId);
	let hours = document.getElementById(hoursId);

	let num = Number.parseInt(hours.innerHTML);
	if (num == 0) num = 23;
	else num--;

	if (num < 10) num = `0${num}`;

	const value = input.value;
	timesSpilt = value.split(":");

	hours.innerHTML = num;
	input.value = num + ":" + timesSpilt[1];
}

function IncMin(inputId, minId) {
	const input = document.getElementById(inputId);
	let mins = document.getElementById(minId);

	let num = Number.parseInt(mins.innerHTML);
	if (num == 59) num = 0;
	else num++;

	if (num < 10) num = `0${num}`;

	const value = input.value;
	timesSpilt = value.split(":");

	mins.innerHTML = num;
	input.value = timesSpilt[0] + ":" + num;
}

function DecMin(inputId, minId) {
	const input = document.getElementById(inputId);
	let mins = document.getElementById(minId);

	let num = Number.parseInt(mins.innerHTML);
	if (num == 0) num = 59;
	else num--;

	if (num < 10) num = `0${num}`;

	const value = input.value;
	timesSpilt = value.split(":");

	mins.innerHTML = num;
	input.value = timesSpilt[0] + ":" + num;
}

function TogglePicker(id) {
	document.getElementById(id).classList.toggle("hide-picker");
}


function HidePicker(id) {
	document.getElementById(id).classList.add("hide-picker");
}