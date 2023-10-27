import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import { IUserLocation } from "../user-location.model";

@Component({
  selector: "jhi-user-location-detail",
  templateUrl: "./user-location-detail.component.html",
})
export class UserLocationDetailComponent implements OnInit {
  userLocation: IUserLocation | null = null;

  constructor(protected activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ userLocation }) => {
      this.userLocation = userLocation;
    });
  }

  previousState(): void {
    window.history.back();
  }
}
