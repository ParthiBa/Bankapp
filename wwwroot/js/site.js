
/*var op; 

function changetextbox() {
	if (document.getElementById("Dropdown").value === "TranferAmount") {
		$("#UserID2").prop("disabled", false);
	} else {
		$("#UserID2").prop("disabled", true);
	}
}


function Submit() {
	if (document.getElementById("Dropdown").value === "DepositAmount") {
		var op = 1;
	} else {
		if (document.getElementById("Dropdown").value === "WithdrawAmount") {
			var op = 2;
		} else {
			if (document.getElementById("Dropdown").value === "TranferAmount") {
				var op = 3;
			}
		}
	}
	console.log(op);
	if (op) {
		$.post('Home/Display', { op: op }, function (output) {
			console.log(output);
		})
	}
}

*/