# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [v0.0.7] - 2025-11-29
### :white_check_mark: Tests
- [`4777879`](https://enduringtech.dev/interflare/orleans-marten/commit/47778791ced74657eec363a33e79916a2888f34f) - *integration*: simplify fixtures *(commit by [@matt](https://enduringtech.dev/matt))*
- [`ba94f92`](https://enduringtech.dev/interflare/orleans-marten/commit/ba94f925c6215097ed56c4596306e2d523a0b4de) - *integration*: make use of testcontainers over manual docker integration *(commit by [@matt](https://enduringtech.dev/matt))*
- [`8fd96d3`](https://enduringtech.dev/interflare/orleans-marten/commit/8fd96d3778c04c8623cab7c3ad4e232c706bc978) - *integration*: move integration test suite project location *(commit by [@matt](https://enduringtech.dev/matt))*

### :wrench: Chores
- [`c7f20e5`](https://enduringtech.dev/interflare/orleans-marten/commit/c7f20e5cbfbded56ee675fb2d40389e5c4077482) - *deps*: update dependency testcontainers.postgresql to v4.9.0 *(commit by [@matt](https://enduringtech.dev/matt))*
- [`7448dd9`](https://enduringtech.dev/interflare/orleans-marten/commit/7448dd944536fa52ecc689be2d5531b6c002b0c5) - *deps*: update dependency riok.mapperly to v4.3.0 *(commit by [@matt](https://enduringtech.dev/matt))*
- [`e49e8f1`](https://enduringtech.dev/interflare/orleans-marten/commit/e49e8f1ea9a83c742aa8b36dd589e63edebb9d15) - *deps*: update dependency microsoft.net.test.sdk to v18.0.1 *(commit by [@matt](https://enduringtech.dev/matt))*
- [`1d86e09`](https://enduringtech.dev/interflare/orleans-marten/commit/1d86e0935329e47bbbd0252bb5d3b15d010e2210) - include .net 10 target *(commit by [@matt](https://enduringtech.dev/matt))*
- [`8698ccd`](https://enduringtech.dev/interflare/orleans-marten/commit/8698ccd60f80efb5435fc1373746b2f520dc4199) - *deps*: update dependency microsoft.aspnetcore.mvc.testing to v9.0.10 *(commit by [@matt](https://enduringtech.dev/matt))*
- [`f71e8a1`](https://enduringtech.dev/interflare/orleans-marten/commit/f71e8a1ead00f120f3550eb1c611b9ba54abef4e) - *deps*: update dependency xunit to v3.1.5 *(commit by [@matt](https://enduringtech.dev/matt))*
- [`c7ae549`](https://enduringtech.dev/interflare/orleans-marten/commit/c7ae549dc5fd19753b8f6893b932be922527119c) - *deps*: update dependency dotnet.reproduciblebuilds to v1.2.39 *(commit by [@matt](https://enduringtech.dev/matt))*
- [`27958a7`](https://enduringtech.dev/interflare/orleans-marten/commit/27958a76359bc9a68918fb568d36a617faa67f42) - *deps*: update dependency microsoft.net.test.sdk to v18 *(PR [#35](https://enduringtech.dev/interflare/orleans-marten/pull/35) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`0a432f5`](https://enduringtech.dev/interflare/orleans-marten/commit/0a432f5a17c5e22ae4711e2985c6c2602225b9cb) - *deps*: update https://enduringtech.dev/actions/checkout-action digest to 08eba0b *(PR [#32](https://enduringtech.dev/interflare/orleans-marten/pull/32) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`7061516`](https://enduringtech.dev/interflare/orleans-marten/commit/706151673547f91ef83cb1083e934eefd3732bd6) - *deps*: update https://enduringtech.dev/actions/release-action action to v1.3.5 *(PR [#31](https://enduringtech.dev/interflare/orleans-marten/pull/31) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`27c1950`](https://enduringtech.dev/interflare/orleans-marten/commit/27c19504282bf83bf4e762a27d63c4a69e041867) - *deps*: update https://enduringtech.dev/actions/release-action action to v1.3.4 *(PR [#29](https://enduringtech.dev/interflare/orleans-marten/pull/29) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`dc5cbbe`](https://enduringtech.dev/interflare/orleans-marten/commit/dc5cbbe8f7a8dced8d7a0952d41df8e971ccdd9d) - *deps*: update https://enduringtech.dev/actions/commit-action action to v6 *(PR [#30](https://enduringtech.dev/interflare/orleans-marten/pull/30) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`845c08f`](https://enduringtech.dev/interflare/orleans-marten/commit/845c08fd1a761c6e1a3acff162a41ae2f56a72dc) - ignore non-major package upgrades in renovate *(commit by [@matt](https://enduringtech.dev/matt))*


## [v0.0.6] - 2025-06-09
### :bug: Bug fixes
- [`f5efb6d`](https://enduringtech.dev/interflare/orleans-marten/commit/f5efb6de1bb8cc88dbd58dca8c87c93bf6002eb6) - accidentally dropped support for .net 8 *(commit by [@matt](https://enduringtech.dev/matt))*


## [v0.0.5] - 2025-06-09
### :sparkles: New features
- [`77c3d4a`](https://enduringtech.dev/interflare/orleans-marten/commit/77c3d4a30bc694a277c39f4b381561ea9f527c50) - use .net 9, marten 8, optimistic concurrency for grain write *(PR [#28](https://enduringtech.dev/interflare/orleans-marten/pull/28) by [@tskimmett](https://enduringtech.dev/tskimmett))*

### :wrench: Chores
- [`83c5ebf`](https://enduringtech.dev/interflare/orleans-marten/commit/83c5ebf4879284a3cc42b76868f1cf4127235715) - *deps*: update dependency marten to 8.0.0 *(commit by [@matt](https://enduringtech.dev/matt))*
- [`3392aa8`](https://enduringtech.dev/interflare/orleans-marten/commit/3392aa854ff3c965bf4c25adb8bb19e711721629) - *deps*: update dependency microsoft.aspnetcore.mvc.testing to 8.0.16 *(PR [#27](https://enduringtech.dev/interflare/orleans-marten/pull/27) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`0fe8d87`](https://enduringtech.dev/interflare/orleans-marten/commit/0fe8d87e641f89d5e1e50f62b9f9ddc7b10f68e7) - *deps*: update dependency marten to 7.40.3 *(PR [#26](https://enduringtech.dev/interflare/orleans-marten/pull/26) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`770769e`](https://enduringtech.dev/interflare/orleans-marten/commit/770769eec378296efafbea5f22ac50f22dc2b8a7) - *deps*: update dependency xunit.runner.visualstudio to 3.1.0 *(PR [#25](https://enduringtech.dev/interflare/orleans-marten/pull/25) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`aebb77b`](https://enduringtech.dev/interflare/orleans-marten/commit/aebb77b176380d7bf07cc915890a28a2ff286252) - *deps*: update dependency marten to 7.40.1 *(PR [#24](https://enduringtech.dev/interflare/orleans-marten/pull/24) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`0165622`](https://enduringtech.dev/interflare/orleans-marten/commit/0165622ba24ba021100d8c0c9ddf744e181cc8b9) - *deps*: update dependency riok.mapperly to 4.2.1 *(PR [#23](https://enduringtech.dev/interflare/orleans-marten/pull/23) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`1c4b4fa`](https://enduringtech.dev/interflare/orleans-marten/commit/1c4b4fa192155e9f7402d477cfa3f639cbed2706) - *deps*: update https://enduringtech.dev/actions/commit-action action to v5.2.0 *(PR [#22](https://enduringtech.dev/interflare/orleans-marten/pull/22) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`a632360`](https://enduringtech.dev/interflare/orleans-marten/commit/a632360d1be21364540986ab8c362d54420a57bb) - *deps*: update dependency marten to 7.40.0 *(PR [#21](https://enduringtech.dev/interflare/orleans-marten/pull/21) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*


## [v0.0.4] - 2025-04-19
### :wrench: Chores
- [`ee5ec6c`](https://enduringtech.dev/interflare/orleans-marten/commit/ee5ec6c9352a8db9582ce8382ec76a476cff60fc) - *deps*: update dependency marten to 7.39.6 *(PR [#17](https://enduringtech.dev/interflare/orleans-marten/pull/17) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`53429de`](https://enduringtech.dev/interflare/orleans-marten/commit/53429deb86c5dc7f7c76579a5f894e1b4d907d7c) - *deps*: update dependency riok.mapperly to 4.2.0 *(PR [#18](https://enduringtech.dev/interflare/orleans-marten/pull/18) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`1cd5b27`](https://enduringtech.dev/interflare/orleans-marten/commit/1cd5b27099ce6cb4f4e3c4224f3006c5b13533c6) - *deps*: update dependency xunit.runner.visualstudio to 3.0.2 *(PR [#16](https://enduringtech.dev/interflare/orleans-marten/pull/16) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`ff8df12`](https://enduringtech.dev/interflare/orleans-marten/commit/ff8df122c42e6482a00165b50f9b2bd8d612dfe2) - *deps*: update orleans monorepo to 9.1.2 *(PR [#19](https://enduringtech.dev/interflare/orleans-marten/pull/19) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`a3e0717`](https://enduringtech.dev/interflare/orleans-marten/commit/a3e0717b5d63293d8b41526bc65056c17293f9bc) - *deps*: update dependency microsoft.aspnetcore.mvc.testing to 8.0.15 *(PR [#14](https://enduringtech.dev/interflare/orleans-marten/pull/14) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`6e62d5f`](https://enduringtech.dev/interflare/orleans-marten/commit/6e62d5ff2ea194c03e42eb307d391826142d9927) - *deps*: pin dependencies *(PR [#13](https://enduringtech.dev/interflare/orleans-marten/pull/13) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`ba95b9b`](https://enduringtech.dev/interflare/orleans-marten/commit/ba95b9b603e26393be799d1ea535a6350e36f921) - configure renovate *(PR [#11](https://enduringtech.dev/interflare/orleans-marten/pull/11) by [@renovate.svc](https://enduringtech.dev/renovate.svc))*
- [`5a10a3c`](https://enduringtech.dev/interflare/orleans-marten/commit/5a10a3c844b769e8015836a32fa95851f922c4f8) - update repository url for new source provider *(commit by [@matt](https://enduringtech.dev/matt))*


## [v0.0.3] - 2025-01-22
### :bug: Bug fixes
- [`01b6631`](https://enduringtech.dev/interflare/orleans-marten/commit/01b663175a8cd7a5be59ed1c337c3621f3104cfe) - **clustering**: return the correct port to the gateway for clients *(commit by [@alfkonee](https://github.com/alfkonee))*


## [v0.0.2] - 2025-01-16
### :bug: Bug fixes
- [`33d5b47`](https://enduringtech.dev/interflare/orleans-marten/commit/33d5b47c0429b750bd0a2d33983b009f97ded3d0) - **clustering**: resolve issue with dependency injection for the gateway list provider (external clients) *(commit by [@matt](https://enduringtech.dev/matt))*
- [`396516e`](https://enduringtech.dev/interflare/orleans-marten/commit/396516ef488f90e400c9931516ff46a25d627e71) - **clustering**: consider the configured orleans service id when listing gateways (external clients) *(commit by [@matt](https://enduringtech.dev/matt))*


## [v0.0.1] - 2025-01-12
### :sparkles: New features
- [`4a5a110`](https://enduringtech.dev/interflare/orleans-marten/commit/4a5a110a5aed40415252b0aa608f599769cafc6b) - marten reminders *(commit by [@matt](https://enduringtech.dev/matt))*
- [`6467af1`](https://enduringtech.dev/interflare/orleans-marten/commit/6467af1ee295a092180088da433b1c603dcf8770) - marten clustering *(commit by [@matt](https://enduringtech.dev/matt))*
- [`dc61cac`](https://enduringtech.dev/interflare/orleans-marten/commit/dc61cac909e6ebc0e13aa7cf3ba5c92d385dedea) - marten grain state persistence *(commit by [@matt](https://enduringtech.dev/matt))*


[v0.0.1]: https://enduringtech.dev/interflare/orleans-marten/compare/v0.0.1-alpha.0...v0.0.1
[v0.0.2]: https://enduringtech.dev/interflare/orleans-marten/compare/v0.0.1...v0.0.2
[v0.0.3]: https://enduringtech.dev/interflare/orleans-marten/compare/v0.0.2...v0.0.3
[v0.0.4]: https://enduringtech.dev/interflare/orleans-marten/compare/v0.0.3...v0.0.4
[v0.0.5]: https://enduringtech.dev/interflare/orleans-marten/compare/v0.0.4...v0.0.5
[v0.0.6]: https://enduringtech.dev/interflare/orleans-marten/compare/v0.0.5...v0.0.6
[v0.0.7]: https://enduringtech.dev/interflare/orleans-marten/compare/v0.0.6...v0.0.7
