(function (contrato) {
    contrato.success = successReload;
    contrato.pages = 1;
    contrato.rowSize = 10;
    /*Atributos para el manejo del Hub*/
    contrato.hub = {};
    contrato.ids = [];
    contrato.recordInUse = false;

    contrato.addContrato = addContratoId;
    contrato.removeHubContrato = removeContratoId;
    contrato.validate = validate;

    $(function () {
        connectToHub();
        init(1);
    })

    return contrato;

    function successReload(option) {
        cibertec.closeModal(option);
        elements = document.getElementsByClassName('active');
        activePage = elements[1].children;
        console.log(activePage[0].text);

        lstTableRows = $('.table >tbody >tr').length - 1;
        console.log(lstTableRows);

        if (option === "delete" && lstTableRows === 1) {
            cant = activePage[0].text;
            init(cant - 1);
        }
        else 
            init(activePage[0].text); 
    }

    function init(numPage) {
        $.get('/Contrato/Count/' + contrato.rowSize,
            function (data) {
                contrato.pages = data;
                $('.pagination').bootpag({
                    total: contrato.pages,
                    page: numPage,
                    maxVisible: 5,
                    leaps: true,
                    firstLastUse: true,
                    first: '← ',
                    last: '→ ',
                    wrapClass: 'pagination',
                    activeClass: 'active',
                    disabledClass: 'disabled',
                    nextClass: 'next',
                    prevClass: 'prev',
                    lastClass: 'last',
                    firstClass: 'first'
                }).on('page', function (event, num) {
                    getContratos(num);
                });
                getContratos(numPage);
            });
    }

    function getContratos(cantPages) {
        var url = '/Contrato/List/' + cantPages + '/' + contrato.rowSize;
        $.get(url, function (data) {
            $('.content').html(data);
            //console.log(data);
        });
    }

    function addContratoId(id) {
        contrato.hub.server.addContratoId(id);
    }

    function removeContratoId(id) {
        contrato.hub.server.removeContratoId(id);
    }

    function connectToHub() {
        contrato.hub = $.connection.contratoHub;
        contrato.hub.client.contratoStatus = contratoStatus;
    }

    function contratoStatus(contratoIds) {
        console.log(contratoIds);
        contrato.ids = contratoIds;
    }

    function validate(id) {
        contrato.recordInUse = (contrato.ids.indexOf(id) > -1);
        if (contrato.recordInUse) {
            $('#inUse').removeClass('hidden');
            $('#btn-save').html("");
        }
    }
})(window.contrato = window.contrato || {});