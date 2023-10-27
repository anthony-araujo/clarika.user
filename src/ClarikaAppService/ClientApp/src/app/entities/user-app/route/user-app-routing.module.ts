import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { UserRouteAccessService } from "app/core/auth/user-route-access.service";
import { UserAppComponent } from "../list/user-app.component";
import { UserAppDetailComponent } from "../detail/user-app-detail.component";
import { UserAppUpdateComponent } from "../update/user-app-update.component";
import { UserAppRoutingResolveService } from "./user-app-routing-resolve.service";

const userAppRoute: Routes = [
  {
    path: "",
    component: UserAppComponent,
    canActivate: [UserRouteAccessService],
  },
  {
    path: ":id/view",
    component: UserAppDetailComponent,
    resolve: {
      userApp: UserAppRoutingResolveService,
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: "new",
    component: UserAppUpdateComponent,
    resolve: {
      userApp: UserAppRoutingResolveService,
    },
    canActivate: [UserRouteAccessService],
  },
  {
    path: ":id/edit",
    component: UserAppUpdateComponent,
    resolve: {
      userApp: UserAppRoutingResolveService,
    },
    canActivate: [UserRouteAccessService],
  },
];

@NgModule({
  imports: [RouterModule.forChild(userAppRoute)],
  exports: [RouterModule],
})
export class UserAppRoutingModule {}
