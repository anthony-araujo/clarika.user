import { Component } from "@angular/core";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";

import { IUserApp } from "../user-app.model";
import { UserAppService } from "../service/user-app.service";

@Component({
  templateUrl: "./user-app-delete-dialog.component.html",
})
export class UserAppDeleteDialogComponent {
  userApp?: IUserApp;

  constructor(
    protected userAppService: UserAppService,
    protected activeModal: NgbActiveModal
  ) {}

  cancel(): void {
    this.activeModal.dismiss();
  }

  confirmDelete(id: number): void {
    this.userAppService.delete(id).subscribe(() => {
      this.activeModal.close("deleted");
    });
  }
}
