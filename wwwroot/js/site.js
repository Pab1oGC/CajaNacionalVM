// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let votes = {}; // Store the doctor's selections

function handleVote(vote, doctorId) {
    votes[`doctor${doctorId}`] = vote;
    const yesButton = document.getElementById(`doctor${doctorId}-yes`);
    const noButton = document.getElementById(`doctor${doctorId}-no`);

    if (vote === 'yes') {
        yesButton.classList.add('btn-yes-active');
        yesButton.classList.remove('btn-vote');
        noButton.classList.remove('btn-no-active');
        noButton.classList.add('btn-vote');
        document.getElementById(`justify-${doctorId}`).style.display = 'none'; // Hide justification
    } else if (vote === 'no') {
        noButton.classList.add('btn-no-active');
        noButton.classList.remove('btn-vote');
        yesButton.classList.remove('btn-yes-active');
        yesButton.classList.add('btn-vote');
        document.getElementById(`justify-${doctorId}`).style.display = 'block'; // Show justification
    }
}

function confirmSubmission() {
    if (!votes.doctor1) {
        Swal.fire({ icon: 'error', title: 'Oops...', text: 'Debes seleccionar una opción de Sí o No para Doctor 1' });
        return;
    }

    let justification = '';
    // Si el voto es 'no', verifica que se haya proporcionado justificación
    if (votes.doctor1 === 'no') {
        justification = document.getElementById('justification-1').value;
        if (!justification) {
            Swal.fire({ icon: 'error', title: 'Justificación requerida', text: 'Debes proporcionar una justificación si seleccionas "No".' });
            return; // No enviar si no hay justificación
        }
    }

    // Confirmación de envío de votos
    Swal.fire({
        title: "¿Estás seguro de enviar los votos?",
        showCancelButton: true,
        confirmButtonText: "Enviar",
        cancelButtonText: "Cancelar",
        icon: "question"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: '/Patients/Vote?handler=SubmitVote', // Asegúrate que esta URL esté correcta
                data: {
                    RequestId: @Model.RequestId, // Usando el RequestId cargado
                    DoctorId: @Model.DoctorId, // Usando el DoctorId cargado
                    Vote: votes.doctor1 === 'yes' ? 'Approved' : 'Pending', // Envía 'Approved' o 'Pending'
                    VoteReazon: votes.doctor1 === 'no' ? justification : null // Solo envía justificación si es 'no'
                },
                success: function (response) {
                    Swal.fire("Votos enviados", "", "success"); // Mensaje de éxito
                },
                error: function (error) {
                    Swal.fire("Error al enviar los votos", "", "error"); // Manejo de error
                }
            });
        } else if (result.isDismissed) {
            Swal.fire("El envío ha sido cancelado", "", "info");
        }
    });
}