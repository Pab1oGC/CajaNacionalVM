let selectIdsDoctor = [];

function AddDoctorDOM(button) {
    const actualRow = button.parentNode.parentNode;
    const table = document.getElementById("table-members").getElementsByTagName("tbody")[0];

    const noMembersMessage = document.getElementById("no-members-message");
    if (noMembersMessage) {
        noMembersMessage.remove();
    }

    const newRow = table.insertRow();
    newRow.setAttribute("origin", actualRow.rowIndex);

    const cellId = newRow.insertCell(0);
    const cellName = newRow.insertCell(1);
    const cellBtn = newRow.insertCell(2);

    cellId.textContent = actualRow.cells[0].textContent;
    cellId.hidden = true;

    cellName.textContent = actualRow.cells[1].textContent;

    const btn = document.createElement("button");
    btn.textContent = "x";
    btn.className = "btn-plus btn-remove";
    btn.onclick = function () { RemoveDoctorDOM(this); };
    cellBtn.appendChild(btn);
    cellBtn.className = "center-cell";

    const idDoctor = actualRow.cells[0].textContent;
    selectIdsDoctor.push(idDoctor);

    actualRow.style.display = "none";
}

function RemoveDoctorDOM(button) {
    const actualRow = button.parentNode.parentNode;
    const table = document.getElementById("table-doctors").getElementsByTagName("tbody")[0];
    const originalIndex = actualRow.getAttribute("origin");

    const realRow = table.rows[originalIndex - 1];
    realRow.style.display = "";

    const idDoctor = actualRow.cells[0].textContent;
    let index = selectIdsDoctor.indexOf(idDoctor);
    selectIdsDoctor.splice(index, 1);

    actualRow.remove();

    if (selectIdsDoctor.length === 0) {
        const tableMembers = document.getElementById("table-members").getElementsByTagName("tbody")[0];
        const noMembersRow = tableMembers.insertRow();
        noMembersRow.id = "no-members-message";
        const cell = noMembersRow.insertCell(0);
        cell.colSpan = 2;
        cell.className = "text-center";
        cell.textContent = "Todavía no existen miembros";
    }
}

function handleEditSubmit(event) {
    const hiddenInput = document.getElementsByName("Ids")[0];
    hiddenInput.value = selectIdsDoctor;
    event.target.submit();
}

async function searchDoctors(query) {
    try {
        const url = query
            ? `/Groups/Edit?handler=SearchDoctors&query=${encodeURIComponent(query)}`
            : `/Groups/Edit?handler=InitialDoctors`;

        const response = await fetch(url);
        const data = await response.json();
        const tableBody = document.getElementById("table-doctors").getElementsByTagName("tbody")[0];

        tableBody.innerHTML = "";
        data.forEach((doctor) => {
            const row = tableBody.insertRow();
            row.innerHTML = `
                <td hidden>${doctor.id}</td>
                <td>${doctor.name} ${doctor.firstName} ${doctor.lastName}</td>
                <td class="center-cell"><button class="btn-plus" onclick="AddDoctorDOM(this)">+</button></td>
            `;
        });
    } catch (error) {
        console.error("Error al buscar doctores:", error);
    }
}

function filterMembers(query) {
    const tableBody = document.getElementById("table-members").getElementsByTagName("tbody")[0];
    const rows = tableBody.getElementsByTagName("tr");

    for (let i = 0; i < rows.length; i++) {
        const cells = rows[i].getElementsByTagName("td");
        if (cells.length > 1) {
            const memberName = cells[1].textContent.toLowerCase();
            if (memberName.includes(query.toLowerCase())) {
                rows[i].style.display = "";
            } else {
                rows[i].style.display = "none";
            }
        }
    }
}
function setIdsValue() {
    const hiddenInput = document.querySelector("[name='Ids']");
    hiddenInput.value = selectIdsDoctor.join(",");
}

