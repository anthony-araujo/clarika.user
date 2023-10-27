import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: "user-app",
        data: { pageTitle: "clarikaAppServiceApp.userApp.home.title" },
        loadChildren: () =>
          import("./user-app/user-app.module").then((m) => m.UserAppModule),
      },
      {
        path: "country",
        data: { pageTitle: "clarikaAppServiceApp.country.home.title" },
        loadChildren: () =>
          import("./country/country.module").then((m) => m.CountryModule),
      },
      {
        path: "state",
        data: { pageTitle: "clarikaAppServiceApp.state.home.title" },
        loadChildren: () =>
          import("./state/state.module").then((m) => m.StateModule),
      },
      {
        path: "city",
        data: { pageTitle: "clarikaAppServiceApp.city.home.title" },
        loadChildren: () =>
          import("./city/city.module").then((m) => m.CityModule),
      },
      {
        path: "location-type",
        data: { pageTitle: "clarikaAppServiceApp.locationType.home.title" },
        loadChildren: () =>
          import("./location-type/location-type.module").then(
            (m) => m.LocationTypeModule
          ),
      },
      {
        path: "user-location",
        data: { pageTitle: "clarikaAppServiceApp.userLocation.home.title" },
        loadChildren: () =>
          import("./user-location/user-location.module").then(
            (m) => m.UserLocationModule
          ),
      },
      /* jhipster-needle-add-entity-route - JHipster will add entity modules routes here */
    ]),
  ],
})
export class EntityRoutingModule {}
