﻿function inicializarFormularioTransacciones(urlObtenerCategorias) {
    $("#TipoOperacionId").change(async function () {
        const valorSeleccionado = $(this).val();

        const respuesta = await fetch(urlObtenerCategorias, {
            method: 'POST',
            body: valorSeleccionado,
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const json = await respuesta.json();
        //console.log(json);
        const opciones = json.map(x => `<option value=${x.value}>${x.text}</option>`);
        $("#CategoriaId").html(opciones);
    });
}