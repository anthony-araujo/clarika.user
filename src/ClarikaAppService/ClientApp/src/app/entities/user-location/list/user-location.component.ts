import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import { IUserLocation } from "../user-location.model";
import { UserLocationService } from "../service/user-location.service";
import { UserLocationDeleteDialogComponent } from "../delete/user-location-delete-dialog.component";

@Component({
  selector: "jhi-user-location",
  templateUrl: "./user-location.component.html",
})
export class UserLocationComponent implements OnInit {
  userLocations?: IUserLocation[];
  isLoading = false;

  constructor(
    protected userLocationService: UserLocationService,
    protected modalService: NgbModal
  ) {}

  loadAll(): void {
    this.isLoading = true;

    this.userLocationService.query().subscribe({
      next: (res: HttpResponse<IUserLocation[]>) => {
        this.isLoading = false;
        this.userLocations = res.body ?? [];
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  ngOnInit(): void {
    this.loadAll();
  }

  trackId(_index: number, item: IUserLocation): number {
    return item.id!;
  }

  delete(userLocation: IUserLocation): void {
    const modalRef = this.modalService.open(UserLocationDeleteDialogComponent, {
      size: "lg",
      backdrop: "static",
    });
    modalRef.componentInstance.userLocation = userLocation;
    // unsubscribe not needed because closed completes on modal close
    modalRef.closed.subscribe((reason) => {
      if (reason === "deleted") {
        this.loadAll();
      }
    });
  }
}
