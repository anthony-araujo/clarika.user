import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

import { IUserLocation } from "../user-location.model";
import { UserLocationService } from "../service/user-location.service";

@Component({
  templateUrl: "./user-location-delete-dialog.component.html",
})
export class UserLocationDeleteDialogComponent {
  userLocation?: IUserLocation;

  constructor(
    protected userLocationService: UserLocationService,
    protected activeModal: NgbActiveModal
  ) {}

  cancel(): void {
    this.activeModal.dismiss();
  }

  confirmDelete(id: number): void {
    this.userLocationService.delete(id).subscribe(() => {
      this.activeModal.close("deleted");
    });
  }
}
