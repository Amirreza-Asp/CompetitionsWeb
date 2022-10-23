function ShowOperationConfirmed() {
	const removeBtn = document.querySelector(".remove-alert-btn");
	const icon = document.createElement("i");
	icon.className = "fa fa-spinner load me-2";
	removeBtn.appendChild(icon);
}

function Delete(url, name) {
	swal({
		title: `${name} مورد نظر حذف شود`,
		text: "امکان بازیابی اطلاعات وجود ندارد",
		type: "warning",
		showCancelButton: true,
		confirmButtonClass: "btn-danger remove-alert-btn",
		confirmButtonText: "حذف",
		cancelButtonText: "بازگشت",
		closeOnConfirm: false,
		closeOnCancel: false
	},
		function (isConfirm) {
			ShowOperationConfirmed();

			if (isConfirm) {
				$.ajax({
					url: url,
					type: "DELETE",
					success: function (data) {
						if (data.success) {
							swal({
								title: "حذف",
								text: "عملیات با موفقیت انجام شد",
								type: "success",
								confirmButtonText: "ثبت"
							},
								function () {
									window.location.reload()
								});
						} else {
							swal("حذف", "عملیات با شکست مواجه شد", "error");
						}
					}
				})
			}
			else {
				swal("عملیات متوقف شد", "بازگشت به لیست", "error")
            }
		}
	);
}
