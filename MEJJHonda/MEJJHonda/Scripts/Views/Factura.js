
var contador = 0;
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
        "IdArticulo": id,
        "Descripcion": descripcion,
        "Cantidad": cantidad,
        "PorcentajeDesc": 0,
        "SubTotal": subtotal,
        "Descuento": 0,
        "Impuesto": subtotal * 0.13
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

function EliminaArticulo(value) {
    $.each(listaArticulos, function (index, campo) {
        if (campo.Id == value) {
            listaArticulos.splice($.inArray(index, campo), 1);
            $('#TableArticulos tfoot tr').eq(value).remove();
            MontosTotales();
        }
    });

}

function MontosTotales() {
    var subTotal = 0;
    var impuesto = 0;
    $.each(listaArticulos, function (index, campo) {

        subTotal += campo.SubTotal;
        impuesto += campo.Impuesto;
    });
    $("#subTotal").html("₡ " + subTotal);
    $("#impuesto").html("₡ " + impuesto);
    $("#total").html("₡ " + (parseFloat(subTotal) + parseFloat(impuesto)));
   }

function Registrar(id) {
    console.log("registrar", idCliente);

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

