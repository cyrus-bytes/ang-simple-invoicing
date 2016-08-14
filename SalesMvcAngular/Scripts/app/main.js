angular.module('invoicing', ['angucomplete-alt', 'ui.bootstrap.datetimepicker', 'ngSanitize'])

.service('customersService', function ($http) {
    var fac = {};
    fac.GetCustomers = function () {
        return $http({
            method: 'GET',
            url: '/invoice/GetCustomers'
        });
    }
    return fac;
}).service('itemsService', function ($http) {
    var fac = {};
    fac.GetItems = function (storeId) {
        return $http({
            method: 'POST',
            url: '/invoice/GetItems',
            data: { 'storeId': storeId }
        });
    }
    return fac;
})
    .service('storeService', function ($http) {
        var fac = {};
        fac.GetStores = function () {
            return $http({
                method: 'GET',
                url: '/invoice/GetStores'
            });
        }
        return fac;
    }).service('invoiceService', function ($http) {
        var fac = {};
        fac.SaveInvoice = function (hd, dt,date) {
            return $http({
                method: 'POST',
                url: '/invoice/SaveInvoice',
                data: { 'invHd': hd, 'invDt': dt,'date':date }
            });
        }
        return fac;
    })

// Main application controller
.controller('InvoiceCtrl', ['$scope', '$http', 'customersService', 'itemsService', 'storeService', 'invoiceService',
  function ($scope, $http, customersService, itemsService, storeService, invoiceService) {
      $scope.ItemPrice = null;
      $scope.itemQuantity = null;
      $scope.itemDiscount = null;
      $scope.itemTax = null;
      $scope.itemId = null;
      $scope.itemName = null;
      $scope.Paid = null;
      $scope.Remain = null;
      $scope.Note = null;

      $scope.date = moment(new Date()).format('DD/MM/YYYY HH:MM'); 
     
      $scope.datepickerOptions = {
          format: 'DD/MM/YYYY',
          language: 'en',
          startDate: "01/10/2012",
          endDate: "31/10/2012",
          autoclose: true,
          weekStart: 0
      }

      // Set defaults
      $scope.currencySymbol = '£';
      $scope.printMode = false;
      $scope.invoiceItems = [];
      $scope.invoiceHeader = {};
      //$scope.invoiceItems.push({
      //        ItemId: 1,
      //        Quantity: 10,
      //        itemName: 'Gadget',
      //        ItemPrice: 9.95,
      //        Tax: 13,
      //        Discount: 10
      //    });


      $scope.stores = [];
      storeService.GetStores().then(function (d) {
          $scope.stores = d.data;
      });
      $scope.SelectedStore = null;
      $scope.afterSelectedStore = function (selected) {
          if (selected) {
            
              $scope.SelectedStore = selected.originalObject;
              $scope.getItems($scope.SelectedStore.Id);
              $scope.clearAddInputs();
          } else {
              $scope.items = [];
          }
      }

      $scope.customers = [];
      customersService.GetCustomers().then(function (d) {
          $scope.customers = d.data;
      });

      $scope.SelectedCustomer = null;
      $scope.afterSelectedCustomer = function (selected) {
          if (selected) {
              $scope.SelectedCustomer = selected.originalObject;
              //alert($scope.SelectedCustomer.CustomerName + ' ' + $scope.SelectedCustomer.Id);
          }
      }

      $scope.items = [];
      $scope.getItems = function (storId) {
          itemsService.GetItems(storId).then(function (d) {
              $scope.items = d.data;
          });
      }
      $scope.SelectedItem = null;

      $scope.afterSelectedItem = function (selected) {
          if (selected) {
              $scope.SelectedItem = selected.originalObject;
              $scope.ItemPrice = $scope.SelectedItem.SellPrice;
              $scope.itemQuantity = 1;
          }
      }


      // Adds an item to the invoice's items
      $scope.addItem = function () {
          if ($scope.SelectedItem !== null && $scope.SelectedItem !== undefined && $scope.itemQuantity > 0) {

              $scope.invoiceItems.push({
                  ItemId: $scope.SelectedItem.Id,
                  Quantity: $scope.itemQuantity,
                  itemName: $scope.SelectedItem.ItemName,
                  ItemPrice: $scope.ItemPrice,
                  Tax: $scope.itemTax,
                  Discount: $scope.itemDiscount,
                  StoreId: $scope.SelectedStore.Id,
                  ItemNet: $scope.itemSubTotal()

              });
              $scope.clearAddInputs();
          } else {
              alertify.error('Please select Item');
          }
      }
      $scope.clearAddInputs = function () {
         $scope.ItemPrice = null;
              $scope.itemQuantity = null;
              $scope.itemDiscount = null;
              $scope.itemTax = null;
              $scope.itemId = null;
              $scope.$broadcast('angucomplete-alt:clearInput', 'itemsAutocomplete');
              $scope.SelectedItem = null;
      };
      $scope.printInfo = function () {
          window.print();
      };

      // Remotes an item from the invoice
      $scope.removeItem = function (item) {
          $scope.invoiceItems.splice($scope.invoiceItems.indexOf(item), 1);
      };

      // Calculates the sub total of the invoice
      $scope.invoiceSubTotal = function () {
          var total = 0.00;
          angular.forEach($scope.invoiceItems, function (item, key) {
              total += (item.Quantity * item.ItemPrice);
          });
          return total;
      };
      $scope.itemSubTotal = function () {
          return ((($scope.ItemPrice * $scope.itemQuantity) - $scope.calculateDiscount()) + $scope.calculateTax());
      };
      $scope.calculateDiscount = function () {
          return (($scope.itemDiscount * $scope.ItemPrice) / 100);
      };
      $scope.calculateTax = function () {
          return (($scope.itemTax * $scope.ItemPrice) / 100);
      };
      // Calculates the tax of the invoice
      //$scope.calculateTax = function() {
      //  return (($scope.invoice.tax * $scope.invoiceSubTotal())/100);
      //};
      $scope.saveInvoice = function () {
          $scope.SaveResult = null;
          var save = true;
          if ($scope.SelectedCustomer == null) {
              alertify.error('Please select Customer');
              save = false;
          }

          if ($scope.invoiceItems.length === 0) {
              alertify.error('Please add items to invoice ');
              save = false;
          }

          if (save) {
              
          $scope.invoiceHeader = {
              invoiceDate: $scope.date,
              Discount: $scope.calculateTotalDiscount(),
              Tax: $scope.calculateTotalTax(),
              Net: $scope.calculateGrandTotal(),
              Paid: $scope.Paid,
              Remaining: $scope.Remain,

              Notes: $scope.Note,
              TypeId: 2,
              CustomerId: $scope.SelectedCustomer.Id
          };

          invoiceService.SaveInvoice($scope.invoiceHeader, $scope.invoiceItems, $scope.date).then(function (d) {
              if (d.data !== "0") {
                  alertify.success('the Invoice is successfully Saved');
                  $scope.Paid = null;
                  $scope.Remain = null;
                  $scope.Note = null;
                  $scope.invoiceHeader = {};
                  $scope.invoiceItems = [];
                  $scope.$broadcast('angucomplete-alt:clearInput', 'customersAutocomplete');
                  $scope.$broadcast('angucomplete-alt:clearInput', 'storesAutocomplete');

                  
                  $scope.SaveResult = d.data;
              } else {
                  alertify.error('the Invoice is not Saved');
              }

          });
          }
      };
    
      // Calculates the grand total of the invoice
      $scope.calculateGrandTotal = function () {
          var total = 0;
          for (var count = 0; count < $scope.invoiceItems.length; count++) {
              total += $scope.invoiceItems[count].ItemNet;
          }
          return total;
      };

      $scope.calculateTotalDiscount = function () {
          var totalDiscount = 0;

          for (var count = 0; count < $scope.invoiceItems.length; count++) {
              totalDiscount += (($scope.invoiceItems[count].Discount * $scope.invoiceItems[count].ItemPrice) / 100);
          }
          return totalDiscount;
      };
      $scope.calculateTotalTax = function () {
          var totalTax = 0;

          for (var count = 0; count < $scope.invoiceItems.length; count++) {
              totalTax += (($scope.invoiceItems[count].Tax * $scope.invoiceItems[count].ItemPrice) / 100);
          }
          return totalTax;
      };
      $scope.calculateTotal = function () {
          var total = 0;
          for (var count = 0; count < $scope.invoiceItems.length; count++) {
              total += ($scope.invoiceItems[count].ItemPrice * $scope.invoiceItems[count].Quantity);
          }
          return total;
      };
      $scope.$watch('Paid', function () {
          $scope.Remain = $scope.calculateGrandTotal() - $scope.Paid;
      });


      var clear = function () {
          $scope.Paid = null;
          $scope.Remain = null;
          $scope.Note = null;
          $scope.invoiceHeader = {};
          $scope.invoiceItems = [];
          $scope.$broadcast('angucomplete-alt:clearInput', 'customersAutocomplete');
          $scope.$broadcast('angucomplete-alt:clearInput', 'storesAutocomplete');
      };

      // Runs on document.ready
      angular.element(document).ready(function () {
          // Set focus
          //document.getElementById('invoice-number').focus();

      });

  }])
