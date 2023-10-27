import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import { ILocationType } from "../location-type.model";
import { LocationTypeService } from "../service/location-type.service";
import { LocationTypeDeleteDialogComponent } from "../delete/location-type-delete-dialog.component";

@Component({
  selector: "jhi-location-type",
  templateUrl: "./location-type.component.html",
})
export class LocationTypeComponent implements OnInit {
  locationTypes?: ILocationType[];
  isLoading = false;

  constructor(
    protected locationTypeService: LocationTypeService,
    protected modalService: NgbModal
  ) {}

  loadAll(): void {
    this.isLoading = true;

    this.locationTypeService.query().subscribe({
      next: (res: HttpResponse<ILocationType[]>) => {
        this.isLoading = false;
        this.locationTypes = res.body ?? [];
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  ngOnInit(): void {
    this.loadAll();
  }

  trackId(_index: number, item: ILocationType): number {
    return item.id!;
  }

  delete(locationType: ILocationType): void {
    const modalRef = this.modalService.open(LocationTypeDeleteDialogComponent, {
      size: "lg",
      backdrop: "static",
    });
    modalRef.componentInstance.locationType = locationType;
    // unsubscribe not needed because closed completes on modal close
    modalRef.closed.subscribe((reason) => {
      if (reason === "deleted") {
        this.loadAll();
      }
    });
  }
}
