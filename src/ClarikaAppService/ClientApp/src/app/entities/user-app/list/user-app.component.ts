import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import { IUserApp } from "../user-app.model";
import { UserAppService } from "../service/user-app.service";
import { UserAppDeleteDialogComponent } from "../delete/user-app-delete-dialog.component";

@Component({
  selector: "jhi-user-app",
  templateUrl: "./user-app.component.html",
})
export class UserAppComponent implements OnInit {
  userApps?: IUserApp[];
  isLoading = false;

  constructor(
    protected userAppService: UserAppService,
    protected modalService: NgbModal
  ) {}

  loadAll(): void {
    this.isLoading = true;

    this.userAppService.query().subscribe({
      next: (res: HttpResponse<IUserApp[]>) => {
        this.isLoading = false;
        this.userApps = res.body ?? [];
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  ngOnInit(): void {
    this.loadAll();
  }

  trackId(_index: number, item: IUserApp): number {
    return item.id!;
  }

  delete(userApp: IUserApp): void {
    const modalRef = this.modalService.open(UserAppDeleteDialogComponent, {
      size: "lg",
      backdrop: "static",
    });
    modalRef.componentInstance.userApp = userApp;
    // unsubscribe not needed because closed completes on modal close
    modalRef.closed.subscribe((reason) => {
      if (reason === "deleted") {
        this.loadAll();
      }
    });
  }
}
