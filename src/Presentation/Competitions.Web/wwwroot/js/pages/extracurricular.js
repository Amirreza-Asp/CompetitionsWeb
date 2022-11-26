
function ClickSubmit(event) {
    const timeBox = document.getElementById("time-box")
    const boxes = timeBox.children;
    if (boxes.length == 0) {
        toastr.error('روز و ساعت برگزاری دوره را وارد کنید')
        event.preventDefault();
    }

    const strValue = document.getElementById("start-register").value;
    if (!strValue) {
        document.getElementById("start-register-validation").style.display = "initial";
        event.preventDefault();
    }

    const enrValue = document.getElementById("end-register").value;
    if (!enrValue) {
        document.getElementById("end-register-validation").style.display = "initial";
        event.preventDefault();
    }

    const spoValue = document.getElementById("start-put-on").value;
    if (!spoValue) {
        event.preventDefault();
        document.getElementById("start-put-on-validation").style.display = "initial";
    }


    const epoValue = document.getElementById("end-put-on").value;
    if (!epoValue) {
        event.preventDefault();
        document.getElementById("end-put-on-validation").style.display = "initial";
    }


    const timeInput = document.getElementById("timesInput");
    timeInput.value = "[ ";
    Array.from(boxes).forEach((box, index) => {
        timeInput.value += box.getAttribute("value");
        if (index != boxes.length - 1)
            timeInput.value += " , ";
    })
    timeInput.value += " ]";
}

function SelectPlace(select) {
    const value = select.value;
    AddSubPlaces(value);
}

function SelectSport(select) {
    const value = select.value;
    AddCoach(value)
}

function SelectSubPlace(select) {
    AddSport(select.value);
}

function AddSport(id) {

    const select = document.getElementById('sports');
    const sportId = document.getElementById("sport-input").value;

    $.ajax({
        url: `/Extracurriculars/Extracurricular/GetSportsByPlaceId/${id}`,
        type: "GET",
        success: function (info) {

            Array.from(select.children).forEach(item => item.remove());

            if (info.exists && info.data.length > 0) {

                if (sportId > 0) {
                    AddCoach(sportId);
                    document.getElementById("sport-input").value = 0;
                }

                Array.from(info.data).forEach((item, index) => {
                    if (sportId <= 0 && index == 0)
                        AddCoach(item.value);

                    let opt = document.createElement("option");
                    opt.value = item.value;
                    opt.text = item.text;
                    if (sportId > 0 && item.value == sportId) {
                        opt.selected = true;
                        AddCoach(sportId);
                    }
                    select.appendChild(opt);
                });

            }
        }
    })


}

function AddSubPlaces(id) {
    const select = document.getElementById('sub-places');

    const placeId = document.getElementById("placeId-input").value;
    document.getElementById("placeId-input").value = "00000000-0000-0000-0000-000000000000";
    $.ajax({
        url: `/Extracurriculars/Extracurricular/GetSubPlacesByPlaceId/${id}`,
        type: "GET",
        success: function (info) {

            Array.from(select.children).forEach(item => item.remove());


            if (info.exists && info.data.length > 0) {

                if (placeId != "00000000-0000-0000-0000-000000000000")
                    AddSport(placeId);

                Array.from(info.data).forEach((item, index) => {
                    if (placeId == "00000000-0000-0000-0000-000000000000" && index == 0)
                        AddSport(item.value);
                    let opt = document.createElement("option");
                    opt.value = item.value;
                    opt.text = item.text;
                    if (opt.value.toLowerCase() == placeId.toLowerCase()) {
                        opt.selected = true;
                    }
                    select.appendChild(opt);
                });

            }
        }
    })
}

function AddCoach(id) {
    const select = document.getElementById('coach');

    $.ajax({
        url: `/Extracurriculars/Extracurricular/GetCoachBySportId/${id}`,
        type: "GET",
        success: function (info) {

            Array.from(select.children).forEach(item => item.remove());

            if (info.exists && info.data.length > 0) {
                select.setAttribute("style", "color:black;")
                Array.from(info.data).forEach(item => {
                    let opt = document.createElement("option");
                    opt.value = item.value;
                    opt.text = item.text;
                    select.appendChild(opt);
                });
            }
            else {

                select.setAttribute("style", "color:red;")
                let opt = document.createElement("option");
                opt.text = "هیچ مربی برای ورزش انتخاب شده وجود ندارد";
                select.appendChild(opt);
            }
        }
    })
}

function AddTime() {
    const timeInput = document.getElementById("timesInput");
    const timePicker = document.getElementById("input-picker");
    const dayPicker = document.getElementById("day-picker");

    if (!dayPicker.value)
        return;

    const day = dayPicker.value;
    const time = timePicker.value;

    AddBox(day, time);
}

function AddBox(day, time) {
    const timeBox = document.getElementById("time-box")

    const div = document.createElement("div");
    div.className = "d-flex flex-column justify-content-center mx-2 border p-2";
    div.setAttribute("style", "box-shadow:inset 0 0 10px gray;border-radius:3px;");
    div.setAttribute("value", `{ day : "${day}" , time : "${time}" }`);

    const spanText = document.createElement("span");
    spanText.className = "px-2 mb-1";
    spanText.innerHTML = `${day} هر هفته ساعت ${time}`;

    const spanRemove = document.createElement("span");
    spanRemove.className = "text-danger mx-auto";
    spanRemove.style.cursor = "pointer";
    spanRemove.innerHTML = "حذف";
    spanRemove.onclick = function () {
        div.remove();
    }

    div.appendChild(spanText);
    div.appendChild(spanRemove);

    timeBox.appendChild(div);
}

window.addEventListener("DOMContentLoaded", () => {
    const placeId = document.getElementById("placeId-input").value;
    if (placeId !== "00000000-0000-0000-0000-000000000000") {

        const select = document.getElementById("place");
        $.ajax({
            url: `/Extracurriculars/Extracurricular/GetParentPlaceByChildPlaceId/${placeId}`,
            type: "GET",
            success: function (info) {

                if (info.exists) {
                    let item = info.data;

                    Array.from(select.children).forEach(opt => {
                        if (opt.value == item.value) {
                            opt.selected = true;
                            AddSubPlaces(opt.value);
                            AddSport(opt.value);
                        }
                    })
                }
            }
        });
    }

});
