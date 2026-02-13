var Address = {
    GetCitiesByStateId: function (StateId) {
        $.ajax({
            type: "GET",
            url: '/Account/GetCitiesByStateId/' + StateId,
            cache: false,
            success: function (data) {
                var items = '<select class="custom-select form-control" id="Address_CityId" name="Address.CityId">';
                $.each(data, function (i, city) {
                    items += '<option value="' + city.uniqueId + '">' + city.name + '</option>'
                });
                items += '</select>';
                $('#Address_CityId').html(items);
            }
        });
    }
    }