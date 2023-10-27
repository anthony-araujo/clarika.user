import { Injectable } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { Resolve, ActivatedRouteSnapshot, Router } from "@angular/router";
import { Observable, of, EMPTY } from "rxjs";
import { mergeMap } from "rxjs/operators";

import { ILocationType, LocationType } from "../location-type.model";
import { LocationTypeService } from "../service/location-type.service";

@Injectable({ providedIn: "root" })
export class LocationTypeRoutingResolveService
  implements Resolve<ILocationType>
{
  constructor(
    protected service: LocationTypeService,
    protected router: Router
  ) {}

  resolve(
    route: ActivatedRouteSnapshot
  ): Observable<ILocationType> | Observable<never> {
    const id = route.params["id"];
    if (id) {
      return this.service.find(id).pipe(
        mergeMap((locationType: HttpResponse<LocationType>) => {
          if (locationType.body) {
            return of(locationType.body);
          } else {
            this.router.navigate(["404"]);
            return EMPTY;
          }
        })
      );
    }
    return of(new LocationType());
  }
}
