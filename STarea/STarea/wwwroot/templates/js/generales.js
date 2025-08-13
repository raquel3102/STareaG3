function ConsultarNombre() {

    let cedula = $("#Cedula").val();
    $("#Nombre").val("");

    if (cedula.length >= 9) {
        $.ajax({
            url: 'https://apis.gometa.org/cedulas/' + cedula,
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                $("#Nombre").val(data.nombre);
            }
        });
    }

}