// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
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

function handleSubmit(event) {
    
    const hiddenInput = document.getElementsByName("ids")[0];
    hiddenInput.value = selectIdsDoctor;
    event.target.submit();
}
async function searchDoctors(query) {
    try {
        const url = query
            ? `/Groups/Create?handler=SearchDoctors&query=${encodeURIComponent(query)}`
            : `/Groups/Create?handler=InitialDoctors`;

        const response = await fetch(url);
        const data = await response.json();
        const tableBody = document.getElementById("table-doctors").getElementsByTagName("tbody")[0];
        tableBody.innerHTML = ""; // Limpiar la tabla actual

        // Obtener los IDs de doctores ya en la lista de miembros
        const membersTableBody = document.getElementById("table-members").getElementsByTagName("tbody")[0];
        const memberIds = Array.from(membersTableBody.children).map(row => row.children[0].textContent);

        const filteredData = data.filter(doctor => !memberIds.includes(doctor.id.toString()));

        if (filteredData.length === 0) {
            const noResultsRow = document.createElement("tr");
            noResultsRow.innerHTML = `<td colspan="3" class="text-center">No se encontraron resultados</td>`;
            tableBody.appendChild(noResultsRow);
        } else {
            filteredData.forEach(doctor => {
                const row = document.createElement("tr");
                row.innerHTML = `
                                    <td hidden>${doctor.id}</td>
                                    <td>${doctor.name} ${doctor.firstName} ${doctor.lastName}</td>
                                    <td class="center-cell"><button class="btn-plus" onclick="AddDoctorDOM(this)">+</button></td>
                                `;
                tableBody.appendChild(row);
            });
        }
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
