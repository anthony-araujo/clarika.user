import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { UserRouteAccessService } from "app/core/auth/user-route-access.service";
import { UserLocationComponent } from "../list/user-location.component";
import { UserLocationDetailComponent } from "../detail/user-location-detail.component";
import { UserLocationUpdateComponent } from "../update/user-location-update.component";
import { UserLocationRoutingResolveService } from "./user-location-routing-resolve.service";

const userLocationRoute: Routes = [
  {
    path: "",
    component: UserLocationComponent,
    canActivate: [UserRouteAccessService],
  },
  {
    path: ":id/view",
    component: UserLocationDetailComponent,
    resolve: {
      userLocation: UserLocationRoutingResolveService,
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: "new",
    component: UserLocationUpdateComponent,
    resolve: {
      userLocation: UserLocationRoutingResolveService,
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: ":id/edit",
    component: UserLocationUpdateComponent,
    resolve: {
      userLocation: UserLocationRoutingResolveService,
    },
    canActivate: [UserRouteAccessService],
  },
];

@NgModule({
  imports: [RouterModule.forChild(userLocationRoute)],
  exports: [RouterModule],
})
export class UserLocationRoutingModule {}
