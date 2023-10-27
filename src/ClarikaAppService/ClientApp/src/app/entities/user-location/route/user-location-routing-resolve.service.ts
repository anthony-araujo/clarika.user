import { Injectable } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { Resolve, ActivatedRouteSnapshot, Router } from "@angular/router";
import { Observable, of, EMPTY } from "rxjs";
import { mergeMap } from "rxjs/operators";

import { IUserLocation, UserLocation } from "../user-location.model";
import { UserLocationService } from "../service/user-location.service";

@Injectable({ providedIn: "root" })
export class UserLocationRoutingResolveService
  implements Resolve<IUserLocation>
{
  constructor(
    protected service: UserLocationService,
    protected router: Router
  ) {}

  resolve(
    route: ActivatedRouteSnapshot
  ): Observable<IUserLocation> | Observable<never> {
    const id = route.params["id"];
    if (id) {
      return this.service.find(id).pipe(
        mergeMap((userLocation: HttpResponse<UserLocation>) => {
          if (userLocation.body) {
            return of(userLocation.body);
          } else {
            this.router.navigate(["404"]);
            return EMPTY;
          }
        })
      );
    }
    return of(new UserLocation());
  }
}
