// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    
    $('.select2').select2({
        allowClear: true,
        placeholder: "Select an option"
    });
    $('[data-mask]').inputmask();
    //$('.select2').select2();


    $('.searchDO').select2({
        minimumInputLength: 3,
        allowClear: true,
        placeholder: 'Search for DO Number',
        ajax: {
            delay: 250,

            url: "/Transaction/StoreClaim/GetAvailableDOStoreClaim",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.name,
                            id: item.id
                        }
                    })
                };
            },
        }
    });

    $('.searchCompany').select2({
        minimumInputLength: 3,
        allowClear: true,
        placeholder: 'Search for Company',
        ajax: {
            delay: 250,

            url: "/Transaction/SPK/GetListCompany",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.code + " " + item.name,
                            id: item.id
                        }
                    })
                };
            },
        }
    });

    $('.searchCostCenter').select2({
        minimumInputLength: 3,
        allowClear: true,
        placeholder: 'Search for Cost Center',
        ajax: {
            delay: 250,

            url: "/Transaction/SPK/GetListCostCenter",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    s: $('#CompanyID').val(),
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.id + " " +item.name,
                            id: item.id
                        }
                    })
                };
            },
        }
    });

    $('.searchVendor').select2({
        minimumInputLength: 3,
        allowClear: true,
        placeholder: 'Search for Vendor',
        ajax: {
            delay: 250,

            url: "/Transaction/SPK/GetListVendor",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    s: $('#CompanyID').val(),
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.id + " " + item.name,
                            id: item.id
                        }
                    })
                };
            },
        }
    });

    $('.searchSBU').select2({
        minimumInputLength: 3,
        allowClear: true,
        placeholder: 'Search for SBU',
        ajax: {
            delay: 250,

            url: "/Transaction/SPK/GetListSBU",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    s: $('#CompanyID').val(),
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.code + " " + item.name,
                            id: item.id
                        }
                    })
                };
            },
        }
    });

    $('.searchStoreCode').select2({
        minimumInputLength: 3,
        allowClear: true,
        placeholder: 'Search for Store Code',
        ajax: {
            delay: 250,

            url: "/Transaction/SPK/GetListStoreCode",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    s: $('#CompanyID').val(),
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.code + " " + item.name,
                            id: item.id
                        }
                    })
                };
            },
        }
    });

    $('.searchAssetType').select2({
        minimumInputLength: 3,
        allowClear: true,
        placeholder: 'Search for Asset Type',
        ajax: {
            delay: 250,

            url: "/Transaction/SPK/GetListAssetClass",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.id + " " + item.name,
                            id: item.id
                        }
                    })
                };
            },
        }
    });

    $('.searchApproval1').select2({
        minimumInputLength: 3,
        placeholder: 'Search for Approval 1 name',
        ajax: {
            delay: 250,

            url: "/Master/User/GetListAD",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.email,
                            id: item.email
                        }
                    })
                };
            },
        }
    });
    $('.searchApproval2').select2({
        minimumInputLength: 3,
        placeholder: 'Search for Approval 2 name',
        ajax: {
            delay: 250,

            url: "/Master/User/GetListAD",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.email,
                            id: item.email
                        }
                    })
                };
            },
        }
    });
    $('.searchApproval3').select2({
        minimumInputLength: 3,
        placeholder: 'Search for Approval 3 name',
        ajax: {
            delay: 250,

            url: "/Master/User/GetApproval",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.email,
                            id: item.email
                        }
                    })
                };
            },
        }
    });
    $('.srchEmailUser').select2({
        minimumInputLength: 3,
        placeholder: 'Search for ActiveDir Email',
        ajax: {
            delay: 250,

            url: "/Master/User/GetListAD",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.email,
                            id: item.email
                        }
                    })
                };
            },
        }
    });

    
    $('.srchEmailUser2').select2({
        minimumInputLength: 3,
        placeholder: 'Search for ActiveDir Email',
        ajax: {
            delay: 250,

            url: "/Master/User/GetListAD",
            type: "GET",
            dataType: 'json',
            data: function (params) {
                var queryParameters = {
                    q: params.term
                }

                return queryParameters;
            },
            processResults: function (data) {
                console.log(data);
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.email,
                            id: item.email
                        }
                    })
                };
            },
        }
    });

});

 

//logout
function Logout() {

    swal({

        title: "Logout from this site?",

        text: "",

        type: "warning",

        showCancelButton: true,

        confirmButtonColor: "#DD6B55",

        confirmButtonText: "Yes",

        closeOnConfirm: false

    }, function () {

        gotoHomeClass();

    });

};



function gotoHomeClass() {

    $.ajax({

        url: '/Register/Logout',

        type: 'POST',

        data: null,

        success: function (data) {

            //window.location.reload(false);
            window.location.href = "/Login/LoginForm";
        }

    });



};

async function ConfirmDialog(val) {

    await swal({

        title: "Are You Sure To " + val + " ?",

        text: "",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: true,
        confirmButtonColor: "#DD6B55"

    }, function (isConfirm) {
        if (isConfirm) {
            var btnApprove = document.getElementById('btnApprove6');
            var btnReject = document.getElementById('btnReject6');
            var btnRevise = document.getElementById('btnRevise6');
            if (val == "Approve") {
                btnApprove.click();
            } else if (val == "Reject") {
                btnReject.click();
            } else {
                btnRevise.click();
            }
        } 
    });

};


function deleteStoreClaim(itemid) {

    swal({

        title: "Are sure want to Delete Store Claim?",

        text: "",

        type: "warning",

        showCancelButton: true,

        confirmButtonColor: "#DD6B55",

        confirmButtonText: "Yes",

        closeOnConfirm: false

    }, function (isConfirm) {
        if (isConfirm) {
            $.ajax({

                url: '/Transaction/StoreClaim/DeleteStoreClaim',
                type: 'POST',
                data: { "id": itemid },
                success: function (data) {
                    swal
                        ({
                            title: "Deleted!",
                            text: "Store Claim has been deleted!",
                            type: "success"
                        },
                        function () {
                            location.reload();
                        });
                }

            });
        }
    });

};

function deleteUser(itemid) {

    swal({

        title: "Are sure want to delete this user?",

        text: "",

        type: "warning",

        showCancelButton: true,

        confirmButtonColor: "#DD6B55",

        confirmButtonText: "Yes",

        closeOnConfirm: false

    }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({

                    url: '/Master/User/Delete',

                    type: 'POST',

                    data: { "id": itemid  },

                    success: function (data) {
                        swal
                            ({
                                title: "Deleted!",
                                text: "User has been deleted!",
                                type: "success"
                            },
                                function () {
                                    window.location.href = '/Master/User/Index';
                                });
                    }

                });
            }
    });

};



function deleteUserNew(itemid) {

    swal({

        title: "Are sure want to delete this user?",

        text: "",

        type: "warning",

        showCancelButton: true,

        confirmButtonColor: "#DD6B55",

        confirmButtonText: "Yes",

        closeOnConfirm: false

    }, function (isConfirm) {
        if (isConfirm) {
            $.ajax({

                url: '/Master/User/Delete',

                type: 'POST',

                data: { "id": itemid },

                success: function (data) {
                    swal
                        ({
                            title: "Deleted!",
                            text: "User has been deleted!",
                            type: "success"
                        },
                            function () {
                                window.location.href = '/Master/User/Index';
                            });
                }

            });
        }
    });

};


function deleteTanggalMerah(itemid) {
    swal({
        title: "Are sure want to delete Tanggal Merah?",
        text: "",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes",
        closeOnConfirm: false

    }, function (isConfirm) {
        if (isConfirm) {
            $.ajax({

                url: '/Master/TanggalMerah/DeleteTanggalMerah',
                type: 'POST',
                data: { "id": itemid },
                success: function (data) {
                    swal
                        ({
                            title: "Deleted!",
                            text: "Tanggal Merah has been deleted!",
                            type: "success"
                        },
                            function () {
                                location.reload();
                            });
                }

            });
        }
    });

};

function ShowLoading() {

    $.blockUI({

        css: {

            border: 'none',

            padding: '15px',

            backgroundColor: '#000',

            '-webkit-border-radius': '10px',

            '-moz-border-radius': '10px',

            opacity: .5,

            color: '#fff'

        }

    });

}

function DisableLoading() {
    $(document).ajaxStop($.unblockUI);
}