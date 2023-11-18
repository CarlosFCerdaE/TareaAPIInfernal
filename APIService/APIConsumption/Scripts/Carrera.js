var tableHdr = null;
var IdRecord = 0;

$(document).ready(function () {
    console.log("antes");
    loadData();
    console.log("despues");

    $('#btnnuevo').on('click', function (e) {
        e.preventDefault();
        IdRecord = 0;
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
        IdRecord = data.ID;
    });

    $('#dt-records').on('click', 'button.btn-delete', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        IdRecord = data.CarreraID;
        if (confirm('¿Seguro de eliminar el registro?')) {
            Eliminar();
        }
    });

});

function loadData() {
    tableHdr = $('#dt-records').DataTable({
        responsive: true,
        destroy: true,
        ajax: "/Carrera/Lista"//,
        //order: [],
        //columns: [
        //    { "data": "ID" },
        //    { "data": "NOMBRE" },
        //    { "data": "FACULTAD" }
        //],
        //processing: true,
        //language: {
        //    "decimal": "",
        //    "emptyTable": "No hay información",
        //    "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
        //    "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
        //    "infoFiltered": "(Filtrado de _MAX_ total entradas)",
        //    "infoPostFix": "",
        //    "thousands": ",",
        //    "lengthMenu": "Mostrar _MENU_ Entradas",
        //    "loadingRecords": "Cargando...",
        //    "processing": "Procesando...",
        //    "search": "Buscar:",
        //    "zeroRecords": "Sin resultados encontrados",
        //    "paginate": {
        //        "first": "Primero",
        //        "last": "Ultimo",
        //        "next": "Siguiente",
        //        "previous": "Anterior"
        //    }
        //},
        //columnDefs: [
        //    {
        //        width: "20%",
        //        targets: 0,
        //        data: "ID"
        //    },
        //    {
        //        width: "39%",
        //        targets: 1,
        //        data: "NOMBRE"
        //    },
        //    {
        //        width: "39%",
        //        targets: 2,
        //        data: "FACULTAD"
        //    },
        //    {
        //        width: "1%",
        //        targets: 4,
        //        data: null,
        //        defaultContent: '<button type="button" class="btn btn-info btn-sm btn-edit" data-target="#modal-record"><i class="fa fa-pencil"></i></button>'
        //    },
        //    {
        //        width: "1%",
        //        targets: 5,
        //        data: null,
        //        defaultContent: '<button type="button" class="btn btn-danger btn-sm btn-delete"><i class="fa fa-trash"></i></button>'

        //    }
        //]
    });
}

function NewRecord() {
    $(".modal-header h3").text("Crear Carrera");

    $('#txtNombreCarrera').val('');
    $('#txtFacultadCarrera').val('');

    $('#modal-record').modal('toggle');
}

function loadDtl(data) {
    $(".modal-header h3").text("Editar Carrera");

    $('#txtNombreCarrera').val(data.NOMBRE);
    $("#txtFacultadCarrera").val(data.FACULTAD);

    $('#modal-record').modal('toggle');
}

function Guardar() {
    var record = "'ID':" + IdRecord;
    record += ",'NOMBRE':'" + $.trim($('#txtCarreraNombre').val()) + "'";
    record += ",'FACULTAD':'" + $.trim($('#txtFacultadCarrera').val()) + "'";

    $.ajax({
        type: 'POST',
        url: '/Carrera/Guardar',
        data: eval('({' + record + '})'),
        success: function (response) {
            if (response.success) {
                $("#modal-record").modal('hide');
                $.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            }
            else {
                $("#modal-record").modal('hide');
                $.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}

function Eliminar() {
    $.ajax({
        type: 'POST',
        url: '/Carrera/Eliminar/?CarreraID=' + IdRecord,
        success: function (response) {
            if (response.success) {
                $.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            } else {
                $.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}