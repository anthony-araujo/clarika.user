import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

import { IUserApp } from "../user-app.model";

@Component({
  selector: "jhi-user-app-detail",
  templateUrl: "./user-app-detail.component.html",
})
export class UserAppDetailComponent implements OnInit {
  userApp: IUserApp | null = null;

  constructor(protected activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ userApp }) => {
      this.userApp = userApp;
    });
  }

  previousState(): void {
    window.history.back();
  }
}
