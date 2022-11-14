import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, Subject } from "rxjs";
import Swal from "sweetalert2";

@Injectable({
    providedIn: 'root'
})

export class AlertService {
    behaviorAlertEvent: BehaviorSubject<boolean>;
    subjectAlertEvent: Subject<boolean>;
    cartCheckoutAlertEvent: Subject<boolean>
    quantityAlertEvent: Subject<number>;
    loggedUserCarRemovalAlertEvent: Subject<boolean>;

    constructor() {
        this.behaviorAlertEvent = new BehaviorSubject<boolean>(false);
        this.subjectAlertEvent = new Subject<boolean>;
        this.quantityAlertEvent = new Subject<number>;
        this.cartCheckoutAlertEvent = new Subject<boolean>;
        this.loggedUserCarRemovalAlertEvent = new Subject<boolean>;
    }

    getLoggedUserCartRemovalEvent() :Observable<boolean> {
      return this.loggedUserCarRemovalAlertEvent.asObservable();
    }

    getBehaviorAlertEvent(): Observable<boolean> {
        return this.behaviorAlertEvent.asObservable();
    }

    getSubjectAlertEvent(): Observable<boolean> {
      return this.subjectAlertEvent.asObservable();
    }

    getQuantityAlertEvent(): Observable<number> {
      return this.quantityAlertEvent.asObservable();
    }

    getCartCheckoutAlertEvent() :Observable<boolean> {
      return this.cartCheckoutAlertEvent.asObservable();
    }

    authGuardForbiddenAlert() {
        Swal.fire({
            icon: 'error',
            title: 'Forbidden',
            text: `You don't have permission to access this page!`,
          })
    }

    authGuardUnauthenticatedAlert() {
      Swal.fire({
          icon: 'error',
          title: 'Unauthenticated',
          text: `You need to login, to access this page!`,
        })
  }

    successfulUserCartRemoval() {
      Swal.fire(
          'User shopping cart - Deleted',
          'No transaction was executed.',
          'success'
        )
  }

    successfulLoggedUserCartRemoval() {
      Swal.fire(
          'Shopping Cart - Deleted',
          'No transaction was executed. You can continue shopping.',
          'success'
        )
  }

    successfulCartCheckout() {
      Swal.fire(
          'Shopping Cart Checkout - Completed!',
          'Thank you for choosing our online shop! You are redirected to the products.',
          'success'
        )
  }

    successfulProductRemoveFromCart() {
      Swal.fire(
          'Product removed from cart successfully',
          'You can navigate to your cart to see your updated products!',
          'success'
        )
  }

    successfulProductAddToCart() {
      Swal.fire(
          'Product added to cart successfully',
          'You can navigate to your cart to see your products!',
          'success'
        )
  }

    successfulProductEdit() {
      Swal.fire(
          'Product edited successfully',
          'You are redirected to the product list',
          'success'
        )
  }

    successfulProductDelete() {
      Swal.fire(
          'Product delete successfully',
          'You are redirected to the product list',
          'success'
        )
  }

    successfulProductCreation() {
      Swal.fire(
          'Product created successfully',
          'Check it out in the product list menus',
          'success'
        )
  }

    successLoggedUserEdit() {
      Swal.fire(
          'You edited your account successfully!',
          'Your account is ready for use! PLEASE log in again!',
          'success'
        )
  }

    successAdminRoleAssignAlert() {
      Swal.fire(
          'Successful role assignment',
          'User is now Administrator',
          'success'
        )
  }

  successAdminRoleUnassignAlert() {
    Swal.fire(
        'Successful role unassignment',
        'User is no longer Administrator',
        'success'
      )
}

    successLoginAlert() {
        Swal.fire(
            'Credentials confirmed!',
            'You are logged IN',
            'success'
          )
    }

    successRegisterAlert() {
      Swal.fire(
          'Registration completed!',
          'You are logged IN',
          'success'
        )
  }

    successLogoutAlert() {
        Swal.fire(
            'Action completed!',
            'You are logged OUT',
            'success'
        )
    }

    errorAlert(error: string) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: error,
          })
    }

    confirmationAlertUserDelete(){
        Swal.fire({
            title: 'Confirm user deletion?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Delete'
          }).then((result) => {
            if (result.isConfirmed) {
              Swal.fire(
                'Deleted!',
                'User deleted.',
                'success'
              )
              this.subjectAlertEvent.next(true);
            } else {
            this.subjectAlertEvent.next(false);
            }
          })
    }

    confirmationAlertUserEdit() {
      Swal.fire({
          title: 'Confirm user edit?',
          text: "Changes can be made again",
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#d33',
          confirmButtonText: 'Save edit'
        }).then((result) => {
          if (result.isConfirmed) {
          this.subjectAlertEvent.next(true);
          } 
        })
  }

  confirmationAlertProductDelete(){
    Swal.fire({
        title: 'Confirm product deletion?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Delete'
      }).then((result) => {
        if (result.isConfirmed) {
          Swal.fire(
            'Deleted!',
            'Product deleted.',
            'success'
          )
          this.subjectAlertEvent.next(true);
        }
      })
}

confirmationAlertProductEdit(){
  Swal.fire({
      title: 'Confirm product edit?',
      text: "You will be returned to product list",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Edit'
    }).then((result) => {
      if (result.isConfirmed) {
        this.subjectAlertEvent.next(true);
      } else {
      this.subjectAlertEvent.next(false);
      }
    })
}

async addToCartDialogBox() {
  const { value: number } = await Swal.fire({
      input: 'number',
      inputLabel: 'Product Quantity',
      inputPlaceholder: 'Enter the desired product quantity',
      inputAttributes: {
        'aria-label': 'Type your message here'
      },
      showCancelButton: true
    })
    var resultAsNumber = Number(number);

    if (resultAsNumber <= 0) {
      Swal.fire("Enter positive number for quantity!")
    }
    
    if (number && resultAsNumber > 0) {
      this.quantityAlertEvent.next(resultAsNumber);
    }
  }

  confirmationAlertRemoveProductFromCart(){
    Swal.fire({
        title: 'Confirm removing the product from cart?',
        text: "You can add it again, through the product list",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Delete'
      }).then((result) => {
        if (result.isConfirmed) {
          this.subjectAlertEvent.next(true);
        }
      })
  }

  confirmationAlertCartCheckout(){
    Swal.fire({
        title: 'Confirm completing the cart checkout process?',
        text: "Your balance will be charged based on the total sum of the cart!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'CHECKOUT'
      }).then((result) => {
        if (result.isConfirmed) {
          this.cartCheckoutAlertEvent.next(true);
        }
      })
  }

  confirmationAlertLoggedUserCartDelete(){
    Swal.fire({
        title: 'Confirm completing the cart removal process?',
        text: "Your cart will be deleted and no payment will be executed.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Remove cart'
      }).then((result) => {
        if (result.isConfirmed) {
          this.loggedUserCarRemovalAlertEvent.next(true);
        }
      })
  }


}