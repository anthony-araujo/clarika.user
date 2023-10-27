import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import { ICity } from "../city.model";
import { CityService } from "../service/city.service";
import { CityDeleteDialogComponent } from "../delete/city-delete-dialog.component";

@Component({
  selector: "jhi-city",
  templateUrl: "./city.component.html",
})
export class CityComponent implements OnInit {
  cities?: ICity[];
  isLoading = false;

  constructor(
    protected cityService: CityService,
    protected modalService: NgbModal
  ) {}

  loadAll(): void {
    this.isLoading = true;

    this.cityService.query().subscribe({
      next: (res: HttpResponse<ICity[]>) => {
        this.isLoading = false;
        this.cities = res.body ?? [];
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  ngOnInit(): void {
    this.loadAll();
  }

  trackId(_index: number, item: ICity): number {
    return item.id!;
  }

  delete(city: ICity): void {
    const modalRef = this.modalService.open(CityDeleteDialogComponent, {
      size: "lg",
      backdrop: "static",
    });
    modalRef.componentInstance.city = city;
    // unsubscribe not needed because closed completes on modal close
    modalRef.closed.subscribe((reason) => {
      if (reason === "deleted") {
        this.loadAll();
      }
    });
  }
}
