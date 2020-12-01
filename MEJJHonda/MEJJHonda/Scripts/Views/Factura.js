
var contador = 0;
var subTotal = 0;
var impuesto = 0;
var total = 0;
var listaArticulos = [];

function SeleccionaArticulo(id, descripcion, precio) {
    $("#articulosModal").modal('hide');
    document.getElementById('IdArticulo').value = id;
    document.getElementById('NameArticulo').value = descripcion;
    document.getElementById('PriceArticulo').value = precio;
}

function ValidaLinea() {
    var id = document.getElementById('IdArticulo').value;
    var cantidad = document.getElementById('CantidadArticulo').value;
    if (id == null || id == "" || cantidad == null || cantidad == "" ) {
    console.log("Debe completar los campos de las líneas de detalle");
    } else {
        if (cantidad <= 0 ) {
    console.log("La cantidad debe de ser mayor a cero");
        } else {
    AnadirLinea();
        }

    }

}

function AnadirLinea() {
    var id = document.getElementById('IdArticulo').value;
    var descripcion = document.getElementById('NameArticulo').value;
    var precio = document.getElementById('PriceArticulo').value;
    var cantidad = document.getElementById('CantidadArticulo').value;
    var subtotal = parseFloat(precio) * parseInt(cantidad);
    listaArticulos.push({
        "Id": contador,
        "IdArticulo": parseInt(id),
        "Descripcion": descripcion,
        "Cantidad": parseInt(cantidad),
        "PorcentajeDesc": 0,
        "SubTotal": parseFloat(precio),
        "Descuento": 0,
        "Impuesto": parseFloat(subtotal * 0.13)
    });
        
    document.getElementById('IdArticulo').value = null;
    document.getElementById('NameArticulo').value = null;
    document.getElementById('PriceArticulo').value = null;
    document.getElementById('CantidadArticulo').value = null;

    contador++;
                var row;
                $.each(listaArticulos, function (index, campo) {
                    row = "<tr>"
                        + "<td>" + campo.Descripcion + "</td>"
                        + "<td>" + campo.IdArticulo + "</td>"
                        + "<td>" + campo.SubTotal + "</td>"
                        + "<td>" + campo.Cantidad + "</td>"
                        + "<td>" + '<a class="btn btn-outline-danger" title="Eliminar línea" onclick="EliminaArticulo(\'' + campo.Id + '\')"> <i class="fas fa-fw fa-minus"></i></a>' + "</td>"
                        + "</tr>";
                });
    $('#TableArticulos tfoot').append(row);
    MontosTotales();
        
           
}

function EliminaArticulo(id) {

    $.each(listaArticulos, function (index, campo) {
        if (campo.Id ===  parseInt(id)) {
            listaArticulos.splice(index, 1);
            $('#TableArticulos tfoot tr').eq(index).remove();
           MontosTotales();
            contador = contador - 1;
        }
    });

}

function MontosTotales() {
    subTotal = 0;
    impuesto = 0;
    $.each(listaArticulos, function (index, campo) {

        subTotal += campo.SubTotal * campo.Cantidad;
        impuesto += campo.Impuesto;
    });
    total = subTotal + impuesto;
    $("#subTotal").html("₡ " + subTotal);
    $("#impuesto").html("₡ " + impuesto);
    $("#total").html("₡ " + total);
   }

function Registrar() {
    if (listaArticulos.length == 0) {
        console.log("La factura no tiene detalles");
    } else {
        var detalles = [];

        $.each(listaArticulos, function (index, campo) {
            detalles.push({
                "IdFacturaD": 0,
                "IdFacturaE": 0,
                "IdArticulo": campo.IdArticulo,
                "Cantidad": campo.Cantidad,
                "PorcentajeDesc": 0,
                "SubTotal": campo.SubTotal,
                "Descuento": 0,
                "Impuesto": campo.Impuesto
            });
        });

    var idCliente = document.getElementById("idCliente").options[document.getElementById("idCliente").selectedIndex].value;
    var observacion = document.getElementById("observacion").value;  



        var encabezado = {
            "IdFacturaE": 0,
            "IdEmpleado": 3,
            "IdCliente": idCliente,
            "Tipo": "Contado",
            "Fecha": new Date().toISOString().slice(0, 10),
            "FechaIng": new Date().toISOString().slice(0, 10),
            "Observacion": observacion,
            "MedioPago": "Transferencia",
            "TipoCambio": 1,
            "Subtotal": subTotal,
            "Descuento": 0,
            "Impuesto": impuesto,
            "IdMoneda": 1
        };  

        $.ajax(
            {
                type: "POST",
                data: JSON.stringify({ mEJJ_FacturaEnca: encabezado, mEJJ_FacturaDeta: detalles}),
                url: "GuardarFactura",
                contentType: 'application/json; charset=utf-8'
            });

       }
}

function ValidaNumericos(evt, input) {
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
}

function filter(__val__) {
    var preg = /^([0-9]+\.?[0-9]{0,3})$/;
    if (preg.test(__val__) === true) {
        return true;
    } else {
        return false;
    }
}

