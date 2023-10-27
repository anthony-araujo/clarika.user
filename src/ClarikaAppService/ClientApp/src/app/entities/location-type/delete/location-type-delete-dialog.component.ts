import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

import { ILocationType } from "../location-type.model";
import { LocationTypeService } from "../service/location-type.service";

@Component({
  templateUrl: "./location-type-delete-dialog.component.html",
})
export class LocationTypeDeleteDialogComponent {
  locationType?: ILocationType;

  constructor(
    protected locationTypeService: LocationTypeService,
    protected activeModal: NgbActiveModal
  ) {}

  cancel(): void {
    this.activeModal.dismiss();
  }

  confirmDelete(id: number): void {
    this.locationTypeService.delete(id).subscribe(() => {
      this.activeModal.close("deleted");
    });
  }
}
