var tableHdr = null;
var IdRecord = "";

$(document).ready(function () {
    loadData();

    $('#btnnuevo').on('click', function (e) {
        e.preventDefault();

        NewRecord();
    });

    $('#btnguardar').on('click', function (e) {
        e.preventDefault();
        Guardar();
    });

    $('#dt-records').on('click', 'button.btn-edit', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        loadDtl(data);
        IdRecord = data.CIF;
    });

    $('#dt-records').on('click', 'button.btn-delete', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        IdRecord = data.CIF;
        if (confirm('¿Seguro de eliminar el registro?')){
            Eliminar();
        }
    });

});

function loadData() {
    tableHdr = $('#dt-records').DataTable({
        responsive: true,
        destroy: true,
        ajax: "/Estudiante/Lista",
        order: [],
        columns: [
            { "data": "CIF" },
            { "data": "NOMBRE" },
            { "data": "APELLIDO" },
            { "data": "CARRERA" }
        ],
        processing: true,
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        columnDefs: [
            {
                width: "18%",
                targets: 0,
                data: "CIF"
            },
            {
                width: "20%",
                targets: 1,
                data: "NOMBRE"
            },
            {
                width: "20%",
                targets: 2,
                data: "APELLIDO"
            },
            {
                width: "20%",
                targets: 3,
                data: "CARRERA"
            },
            {
                width: "1%",
                targets: 4,
                data: null,
                defaultContent: '<button type="button" class="btn btn-info btn-sm btn-edit" data-target="#modal-record"><i class="fa fa-pencil"></i></button>'
            },
            {
                width: "1%",
                targets: 5,
                data: null,
                defaultContent: '<button type="button" class="btn btn-danger btn-sm btn-delete"><i class="fa fa-trash"></i></button>'

            }
        ]
    });
}

function NewRecord() {
    $(".modal-header h3").text("Crear Estudiante");

    $('#txtCifEstudiante').val('');
    $('#txtCifEstudiante').prop('disabled', false);
    $('#txtNombreEstudiante').val('');
    $('#txtApellidoEstudiante').val('');
    $('#txtCarreraEstudiante').val('');

    $('#modal-record').modal('toggle');
}

function loadDtl(data) {
    $(".modal-header h3").text("Editar Estudiante");

    $('#txtCifEstudiante').val(data.CIF);
    $('#txtCifEstudiante').prop('disabled', true);
    $('#txtNombreEstudiante').val(data.NOMBRE);
    $("#txtApellidoEstudiante").val(data.APELLIDO);
    $("#txtCarreraEstudiante").val(data.CARRERA);

    $('#modal-record').modal('toggle');
}

function Guardar() {
    var record = "'CIF':'" + $.trim($('#txtCifEstudiante').val())+"'";
    record += ",'NOMBRE':'" + $.trim($('#txtNombreEstudiante').val()) + "'";
    record += ",'APELLIDO':'" + $.trim($('#txtApellidoEstudiante').val()) + "'";
    record += ",'CARRERA':'" + $.trim($('#txtCarreraEstudiante').val()) + "'";
    console.log(record);    

    $.ajax({
        type: 'POST',
        url: '/Estudiante/Guardar',
        data: eval('({' + record + '})'),
        success: function (response) {
            if (response.success) {
                console.log("success")
                $("#modal-record").modal('hide');
                //$.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            }
            else {
                $("#modal-record").modal('hide');
                console.log("no success")
                //$.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}

function Eliminar() {
    console.log(IdRecord)
    $.ajax({
        type: 'POST',
        url: '/Estudiante/Eliminar/?CIF=' + IdRecord,
        success: function (response) {
            if (response.success) {
                console.log("deleted")
                //$.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            } else {
                console.log("no success to delete")
                //$.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('AJAX Error:', textStatus, errorThrown);
        }
    });
}