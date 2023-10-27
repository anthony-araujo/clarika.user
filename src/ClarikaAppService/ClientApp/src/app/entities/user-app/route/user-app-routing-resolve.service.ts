import { Injectable } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { Resolve, ActivatedRouteSnapshot, Router } from "@angular/router";
import { Observable, of, EMPTY } from "rxjs";
import { mergeMap } from "rxjs/operators";

import { IUserApp, UserApp } from "../user-app.model";
import { UserAppService } from "../service/user-app.service";

@Injectable({ providedIn: "root" })
export class UserAppRoutingResolveService implements Resolve<IUserApp> {
  constructor(protected service: UserAppService, protected router: Router) {}

  resolve(
    route: ActivatedRouteSnapshot
  ): Observable<IUserApp> | Observable<never> {
    const id = route.params["id"];
    if (id) {
      return this.service.find(id).pipe(
        mergeMap((userApp: HttpResponse<UserApp>) => {
          if (userApp.body) {
            return of(userApp.body);
          } else {
            this.router.navigate(["404"]);
            return EMPTY;
          }
        })
      );
    }
    return of(new UserApp());
  }
}
